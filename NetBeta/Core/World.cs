using System;
using System.Numerics;
using NetBeta.Core.Entities;
using NetBeta.IO;
using NetBeta.IO.Tags;
using NetBeta.IO.Util;
using NetBeta.Net;
using NetBeta.Net.Packets;

namespace NetBeta.Core;

public class World
{
    public int EntityCounter = 0;
    public long RandomSeed;
    public int SpawnX;
    public int SpawnY;
    public int SpawnZ;
    public long Time;

    public List<ServerPlayer> players = new();
    public List<Entity> entities = new();

    public List<Chunk> SpawnChunks = new();

    public World(Server server)
    {
        NBT WorldDat = new("/Users/tadwalter/Documents/NetBeta/NetBeta/bin/Debug/net9.0/world/level.dat");
        Compound LevelData = WorldDat.Root.GetCompound("Data");

        RandomSeed = LevelData.Get("RandomSeed");
        SpawnX = LevelData.Get("SpawnX");
        SpawnY = LevelData.Get("SpawnY");
        SpawnZ = LevelData.Get("SpawnZ");
        Time = LevelData.Get("Time");

        GetSpawnChunks(SpawnX, SpawnZ);
        //_ = TimeTick();
    }

    private void GetSpawnChunks(int x, int z)
    {
        for (int DistX = -10; DistX <= 10; DistX++)
        {
            for (int DistZ = -10; DistZ <= 10; DistZ++)
            {
                //We fetch the chunk.
                Chunk chunk = new();
                int ChunkX = (x >> 4) + DistX;
                int ChunkZ = (z >> 4) + DistZ;

                string ChunkFile = $"{Converter.Base36Encode((byte)ChunkX % 64)}/{Converter.Base36Encode((byte)ChunkZ % 64)}/c.{Converter.Base36Encode(ChunkX)}.{Converter.Base36Encode(ChunkZ)}.dat";

                NBT ChunkDat = new("/Users/tadwalter/Documents/NetBeta/NetBeta/bin/Debug/net9.0/world/" + ChunkFile);
                Compound ChunkLevel = ChunkDat.Root.GetCompound("Level");
                chunk.xPos = ChunkLevel.Get("xPos");
                chunk.zPos = ChunkLevel.Get("zPos");
                //chunk.TerrainPopulated = ChunkLevel.Get("TerrainPopulated");
                chunk.LastUpdated = ChunkLevel.Get("LastUpdate");
                chunk.Blocks = ChunkLevel.Get("Blocks");
                chunk.BlockData = ChunkLevel.Get("Data");
                chunk.BlockLight = ChunkLevel.Get("BlockLight");
                chunk.SkyLight = ChunkLevel.Get("SkyLight");
                chunk.HeightMap = ChunkLevel.Get("HeightMap");

                SpawnChunks.Add(chunk);
            }
        }
    }

    public Pickup AddPickupSpawn(Vector3 pos, short type, byte count)
    {
        Pickup entity = new()
        {
            Position = pos,
            Item = type,
            Count = count
        };

        AddEntity(entity);
        return entity;
    }
    
    public void AddEntity(Entity entity)
    {
        entity.SetEntityID(Interlocked.Increment(ref EntityCounter));

        entities.Add(entity);
    }
}
