using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using NetBeta.Core;
using NetBeta.Core.Entities;
using NetBeta.Net.Packets;

namespace NetBeta.Net;

public class Server
{
    World world;
    string ipAddress;
    int Port;

    TcpListener tcpListener;

    public Server(string ipAddress, int Port)
    {
        //new(IPAddress.Parse(ipAddress), Port);
        tcpListener = new(IPAddress.Parse(ipAddress), Port);
        this.ipAddress = ipAddress;
        this.Port = Port;

        this.world = new(this);
    }

    public async Task StartServer()
    {
        tcpListener.Start();
        Console.WriteLine($"NetBeta Server started on {ipAddress}:{Port}");
        _ = Task.Run(() => Tick());
        await Receive();
    }

    private async Task Receive()
    {
        while (true)
        {
            var Client = await tcpListener.AcceptTcpClientAsync();

            //Run this as a seperate task so we can continue
            //Getting clients.
            _ = Task.Run(() => AcceptClient(Client));
        }
    }

    public async Task SendToAllPlayers(Packet packet)
    {
        if (world == null || world.players.Count == 0)
            return;
        
        foreach (ServerPlayer serverPlayer in world.players)
        {
            serverPlayer.SendQueue.Add(packet);
        }
    }
    
    public async Task SendToMost(Packet packet, ServerPlayer player)
    {
        foreach (ServerPlayer serverPlayer in world.players)
        {
            if(serverPlayer != player)
            {
                if (!serverPlayer.Connected)
                    return;
                
                serverPlayer.SendQueue.Add(packet);
            }
        }
    }

    private async Task AcceptClient(TcpClient tcpClient)
    {
        ServerPlayer serverPlayer = new(tcpClient, this);
        serverPlayer.SetEntityID(Interlocked.Increment(ref world.EntityCounter));

        //TODO: Make players private so we can just add the functions like Add and Get.
        world.players.Add(serverPlayer);
        _ = Task.Run(serverPlayer.Receive);
    }

    private async Task Send()
    {
        foreach (ServerPlayer serverPlayer in world.players)
        {
            _ = Task.Run(() => serverPlayer.Send());
        }
    }

    int TimeTickCounter = 0;
    long TEST = 0L;
    public async Task TimeTick()
    {
        TimeTickCounter++;
        //world.Time+=(long)0.0001;
        Console.WriteLine($"Time: {world.Time}");
        world.Time++;
        
        if(TimeTickCounter >= 20)
        {
            TimeTickCounter = 0;
            TEST += 1;
            await SendToAllPlayers(new Time(TEST));
        }
    }

    public async Task Tick()
    {
        while (true)
        {
            Thread.Sleep(50);
            //await TimeTick();
            //await Send();
            //await PlayerReceive();
        }
    }
    
    public World GetWorld()
    {
        return world;
    }
}
