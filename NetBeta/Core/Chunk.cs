using System;

namespace NetBeta.Core;

public class Chunk
{
    public int xPos;
    public int zPos;
    public bool TerrainPopulated;
    public long LastUpdated;
    public byte[] Blocks;
    public byte[] BlockData;
    public byte[] BlockLight;
    public byte[] SkyLight;
    public byte[] HeightMap;
}
