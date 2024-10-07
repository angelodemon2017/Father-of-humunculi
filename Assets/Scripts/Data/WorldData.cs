using System.Collections.Generic;
using System.Linq;

public class WorldData
{
    public string Name;
    public string Seed;

    public List<WorldChunkData> worldChunkDatas = new();
    public List<WorldTileData> worldTileDatas = new();

    public List<EntityData> entityDatas = new();

/*    public WorldChunkData GetChunk(int x, int z)
    {
        var chunkResult = worldChunkDatas.FirstOrDefault(c => c.Xpos == x && c.Zpos == z);

        if (chunkResult == null)
        {
            //            chunkResult = WorldConstructor.GenerateChunk(x, z, Seed);

            //            worldChunkDatas.Add(chunkResult);
        }

        return chunkResult;
    }/**/

    public List<WorldTileData> GetChunk(int x, int z)
    {
        List<WorldTileData> result = new();
        for (var _x = 0; _x < Config.ChunkTilesSize; _x++)
            for (var _z = 0; _z < Config.ChunkTilesSize; _z++)
            {
                var xpos = x * Config.ChunkTilesSize + _x;
                var zpos = z * Config.ChunkTilesSize + _z;
                var tile = GetWorldtileData(xpos, zpos);

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
                result.Add(GetWorldtileData(x + _x, z + _z));
            }

        return result;
    }

    private WorldTileData GetWorldtileData(int x ,int z)
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

    public List<TileMaskData> tileMaskDatas = new();

    public WorldTileData()
    {

    }

    public WorldTileData(int id, int xpos, int zpos)
    {
        Id = id;
        Xpos = xpos;
        Zpos = zpos;
    }
}

public class TileMaskData
{
    public int IdTypeMask;
    public int IndexTextureMask;
    public EnumTileDirect Rotate;
}