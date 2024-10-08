using System.Collections.Generic;
using System.Linq;

public class WorldData
{
    public string Name;
    public string Seed;

    public List<WorldTileData> worldTileDatas = new();

    public List<EntityData> entityDatas = new();

    public List<WorldTileData> GetChunk(int x, int z)
    {
        List<WorldTileData> result = new();
        for (var _x = 0; _x < Config.ChunkTilesSize; _x++)
            for (var _z = 0; _z < Config.ChunkTilesSize; _z++)
            {
                var xpos = x * Config.ChunkTilesSize + _x;
                var zpos = z * Config.ChunkTilesSize + _z;
                var tile = GetWorldTileData(xpos, zpos);

                result.Add(tile);
            }

        return result;
    }

    public List<WorldTileData> GetNeigborsTiles(int x, int z)
    {
        List<WorldTileData> result = new();

        for (int _x = -1; _x < 2; _x++)
            for (int _z = -1; _z < 2; _z++)
            {
                if (_x == 0 && _z == 0)
                {
                    continue;
                }
                result.Add(GetWorldTileData(x + _x, z + _z));
            }

        return result;
    }

    private WorldTileData GetWorldTileData(int x ,int z)
    {
        var tile = worldTileDatas.FirstOrDefault(t => t.Xpos == x && t.Zpos == z);
        if (tile == null)
        {
            tile = WorldConstructor.GenerateTile(x, z);
            worldTileDatas.Add(tile);
        }

        return tile;
    }
}

public class WorldChunkData
{//TODO есть ли смысл в существовании этого класса?
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
    public int SeedMask;

    public WorldTileData(int id, int xpos, int zpos)
    {
        Id = id;
        Xpos = xpos;
        Zpos = zpos;
        SeedMask = UnityEngine.Random.Range(0, int.MaxValue);
    }
}