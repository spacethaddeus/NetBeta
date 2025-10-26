using System.Collections.Concurrent;
using System.IO.Compression;
using System.Net.Sockets;
using NetBeta.Core;
using NetBeta.Core.Entities;
using NetBeta.Net.Packets;

namespace NetBeta.Net;

public class ServerPlayer(TcpClient tcpClient, Server server) : Player
{
    TcpClient tcpClient = tcpClient;
    Server server = server;
    readonly World world = server.GetWorld();
    readonly BinaryReader reader = new(tcpClient.GetStream());
    //TEMP for writer
    public readonly BinaryWriter writer = new(tcpClient.GetStream());
    public BlockingCollection<Packet> SendQueue = [];

    Random random = new();

    public bool Login = false;
    public bool Connected = false;

    private async Task SendKeepAlive()
    {
        while(true)
        {
            SendQueue.Add(new KeepAlive());
            await Task.Delay(10000);
        }
    }
    
    public async Task Receive()
    {
        _ = Task.Run(Send);
        while(true)
        {
            if (Connected == false && Login == true)
                break;
            byte PacketID = (byte)reader.Read();

            switch (PacketID)
            {
                case (byte)Packet.PacketTypes.KeepAlive:
                    await KeepAlive();
                    break;
                case (byte)Packet.PacketTypes.Handshake:
                    if (Login)
                        return;
                    await HandleHandshake();
                    break;
                case (byte)Packet.PacketTypes.Login:
                    if (Login)
                        return;
                    await HandleLogin();
                    break;
                case (byte)Packet.PacketTypes.ChatMessage:
                    await HandleChatMessage();
                    break;
                case (byte)Packet.PacketTypes.PlayerDigging:
                    await HandlePlayerDigging();
                    break;
                case (byte)Packet.PacketTypes.UseEntity:
                    await HandleUseEntity();
                    break;
                case (byte)Packet.PacketTypes.Player:
                    await HandlePlayer();
                    break;
                case (byte)Packet.PacketTypes.PlayerPosition:
                    await HandlePosition();
                    break;
                case (byte)Packet.PacketTypes.PlayerLook:
                    await HandleLook();
                    break;
                case (byte)Packet.PacketTypes.PlayerPositionLook:
                    await HandlePositionLook();
                    break;
                case (byte)Packet.PacketTypes.Animation:
                    await HandleAnimate();
                    break;
                case (byte)Packet.PacketTypes.DisconnectKick:
                    Connected = false;
                    return;
                default:
                    Console.WriteLine($"Packet not implemented! {Enum.GetName(typeof(Packet.PacketTypes), PacketID)} : {PacketID}");
                    break;
            }
        }
    }

    public async Task Send()
    {
        while(true)
        {
            var packet = SendQueue.Take();
            ThreadPool.QueueUserWorkItem(async state =>
            {
                var data = (Packet)state!;
                //await tcpClient.SendAsync(data.Send());
                writer.Write(data.Send());
                
            }, packet );
        }
    }

    public async Task Disconnect()
    {
        tcpClient.Close();

        world.players.Remove(this);
    }

    private async Task KeepAlive()
    {
        KeepAlive keepAlive = new();
        SendQueue.Add(keepAlive);
    }

    private async Task HandleHandshake()
    {
        Handshake handshake = new();
        handshake.Load(reader);

        Username = handshake.Username;

        SendQueue.Add(handshake);
    }

    private async Task HandleLogin()
    {
        //First we get the login information.
        Login login = new(GetEntityID(), world.RandomSeed, 0);
        login.Load(reader);

        if (login.PlayerProtocolVersion != 8)
            Console.WriteLine($"Kicking user because wrong Minecraft Version!");

        //Then, we can send the login response.
        SendQueue.Add(login);

        //Now, we can now create and send chunks. So, we are doing this in a seperate function.
        await FetchChunks(world.SpawnX, world.SpawnY, world.SpawnZ, true);

        //Second to last, we send the spawn position.
        SendQueue.Add(new SpawnPosition(world.SpawnX, world.SpawnY, world.SpawnZ));

        //Lastly, we create and send Player Position and Look to let the client know that we are now in the server.
        PlayerPositionLook playerPositionLook = new()
        {
            X = world.SpawnX,
            Y = world.SpawnY + 2,
            Stance = 0D,
            Z = world.SpawnZ,
            Yaw = 0F,
            Pitch = 0F,
            OnGround = true,
        };

        SendQueue.Add(playerPositionLook);

        Login = true;
        Connected = true;

        foreach (ServerPlayer player in world.players)
        {
            if (player != this)
            {
                Console.WriteLine($"Other Player: {player.GetEntityID()}");
                SendQueue.Add(new NamedEntitySpawn(player.GetEntityID(), player.Username, (int)player.Position.X,
                (int)player.Position.Y, (int)player.Position.Z, (byte)player.Look.X, (byte)player.Look.Y, 0));
            }
        }

        Console.WriteLine($"Your Player: {GetEntityID()}");
        await server.SendToMost(new NamedEntitySpawn(GetEntityID(), Username, (int)Position.X,
                (int)Position.Y, (int)Position.Z, (byte)Look.X, (byte)Look.Y, 0), this);
        await server.SendToMost(new EntityTeleport(GetEntityID(), (int)Position.X, (int)Position.Y, (int)Position.Z,
        (byte)Look.X, (byte)Look.Y), this);
        

        _ = SendKeepAlive();
    }

    private async Task HandleChatMessage()
    {
        ChatMessage chatMessage = new();
        chatMessage.Load(reader);

        chatMessage.Message = $"<{Username}> {chatMessage.GetMessage()}";

        await server.SendToAllPlayers(chatMessage);
    }

    private async Task HandlePlayerDigging()
    {
        PlayerDigging playerDigging = new();
        playerDigging.Load(reader);

        if (playerDigging.Status == 3)
        {
            Pickup entity = world.AddPickupSpawn(
                new System.Numerics.Vector3(playerDigging.X, playerDigging.Y + 1, playerDigging.Z), 1, 1);

            await server.SendToAllPlayers(new PickupSpawn(entity));
            await server.SendToAllPlayers(new BlockChange(playerDigging.X, playerDigging.Y, playerDigging.Z, 0, 0));
        }

        //await Send(playerDigging);
    }
    
    private async Task HandleUseEntity()
    {
        UseEntity useEntity = new();
        useEntity.Load(reader);

        foreach (Entity entity in world.entities)
        {
            if (useEntity.Target == entity.GetEntityID())
            {
                Console.WriteLine("Found entity!");
            }
        }
        //Not found in entity? Let's see for players.
        foreach (ServerPlayer player in world.players)
        {
            if (useEntity.Target == player.GetEntityID())
            {
                if(useEntity.LeftClick == true)
                {
                    //We can't check what we are holding right now.
                    //So, damage them one instead.
                    int Health = player.GetHealth();
                    Health--;
                    player.SetHealth(Health);
                    player.SendQueue.Add(new UpdateHealth((short)Health));

                    if (Health <= 0)
                    {
                        await server.SendToAllPlayers(new EntityStatus(player.GetEntityID(), 3));
                        await Task.Delay(1100);
                        await server.SendToAllPlayers(new DestoryEntity(player.GetEntityID()));
                    }
                    else
                    {
                        await server.SendToAllPlayers(new EntityStatus(player.GetEntityID(), 2));
                    }

                    Animation animation = new()
                    {
                        EntityID = player.GetEntityID(),
                        Animate = 2
                    };
                    await server.SendToAllPlayers(animation);
                    break;
                }
            }
        }
    }

    private async Task HandlePlayer()
    {
        PlayerPacket player = new();
        player.Load(reader);

        OnGround = player.OnGround;
    }

    private async Task HandlePosition()
    {
        PlayerPosition playerPosition = new();
        playerPosition.Load(reader);

        PrevPosition = Position;
        Position = new((float)playerPosition.X, (float)playerPosition.Y, (float)playerPosition.Z);
        OnGround = playerPosition.OnGround;

        //await server.SendToMost(new EntityRelativeMove(GetEntityID(), (byte)playerPosition.X, (byte)playerPosition.Y, (byte)playerPosition.Z), this);
    }

    private async Task HandleLook()
    {
        PlayerLook playerLook = new();
        playerLook.Load(reader); 

        PrevLook = Look;
        Look = new(playerLook.Yaw, playerLook.Pitch);
        OnGround = playerLook.OnGround;

        
        await GetDistance();
    }

    private async Task HandlePositionLook()
    {
        PlayerPositionLook playerPositionLook = new();
        playerPositionLook.Load(reader);

        PrevPosition = Position;
        Position = new((float)playerPositionLook.X, (float)playerPositionLook.Y, (float)playerPositionLook.Z);

        PrevLook = Look;
        Look = new(playerPositionLook.Yaw, playerPositionLook.Pitch);

        OnGround = playerPositionLook.OnGround;

        await server.SendToMost(new EntityTeleport(GetEntityID(), (int)playerPositionLook.X, (int)playerPositionLook.Y, (int)playerPositionLook.Z
        ,(byte)Look.X, (byte)Look.Y), this);

        await GetDistance();
    }

    private async Task HandleAnimate()
    {
        Animation animation = new();
        animation.Load(reader);

        await server.SendToMost(animation, this);
    }

    public async Task GetDistance()
    {

    }
    
    public async Task LoadOrUnloadChunk(Chunk chunk, int x, int z, bool Mode)
    {
        //We can now send the pre chunk.
        SendQueue.Add(new PreChunk(x, z, Mode));

        if (Mode == false)
            return;

        using MemoryStream stream = new();
        stream.Write(chunk.Blocks);
        stream.Write(chunk.BlockData);
        stream.Write(chunk.BlockLight);
        stream.Write(chunk.SkyLight);
        stream.Close();
        byte[] Data = stream.ToArray();

        using MemoryStream ZLibMemoryStream = new();
        using ZLibStream zLibStream = new(ZLibMemoryStream, CompressionLevel.Optimal);
        zLibStream.Write(Data);
        zLibStream.Close();

        byte[] CompressedData = ZLibMemoryStream.ToArray();

        SendQueue.Add(new MapChunk(x << 4, (short)0, z << 4, CompressedData));
    }
    
    public async Task FetchChunks(int x, int y, int z, bool Mode)
    {
        for(int DistX = -10; DistX <= 10; DistX++)
        {
            for (int DistZ = -10; DistZ <= 10; DistZ++)
            {
                Chunk chunk = new();
                int ChunkX = (x >> 4) + DistX;
                int ChunkY = y >> 7;
                int ChunkZ = (z >> 4) + DistZ;

                try
                {
                    foreach (Chunk chunk1 in world.SpawnChunks)
                    {
                        if (chunk1.xPos == ChunkX && chunk1.zPos == ChunkZ)
                        {
                            chunk = chunk1;
                        }
                    }
                }
                catch
                {
                    //We need to fetch a chunk
                    return;
                }


                await LoadOrUnloadChunk(chunk, ChunkX, ChunkZ, Mode);
            }
        }
    }
}
