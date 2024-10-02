using System.Collections.Generic;
using System.Linq;

public class WorldData
{
    public string Name;
    public string Seed;

    public List<WorldChunkData> worldChunkDatas = new();
    public List<WorldTileData> worldTileDatas = new();

    public List<EntityData> entityDatas = new();

    public WorldChunkData GetChunk(int x, int z)
    {
        var chunkResult = worldChunkDatas.FirstOrDefault(c => c.Xpos == x && c.Zpos == z);

        if (chunkResult == null)
        {
            chunkResult = WorldConstructor.GenerateChunk(x, z, Seed);

            worldChunkDatas.Add(chunkResult);
        }

        return chunkResult;
    }
}

public class WorldChunkData
{
    public int Xpos;
    public int Zpos;
    public List<WorldTileData> tiles = new();

    public WorldChunkData(int x, int z)
    {
        Xpos = x;
        Zpos = z;
    }
}

public class WorldTileData
{
    public int Id;
    public int Xpos;
    public int Zpos;
}