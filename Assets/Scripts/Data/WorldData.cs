using System.Collections.Generic;
using System.Linq;

public class WorldData
{
    public string Name;
    public SeedData Seed = new();

    public List<WorldTileData> worldTileDatas = new();
    public List<WorldChunkData> worldChunkDatas = new();//labels about loaded??

    public List<EntityData> entityDatas = new();

    public long lastIds = 0;
    public long GetNewId () 
    {
        lastIds++;
        return lastIds; 
    }
    public List<long> needUpdates = new();

    public WorldData()
    {
        StartGeneration();
    }

    private void StartGeneration()
    {
        for (int x = -2; x < 3; x++)
            for (int z = -2; z < 3; z++)
            {
                GetChunk(x, z);
            }
    }

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

        if (!worldChunkDatas.Any(c => c.Xpos == x && c.Zpos == z))
        {
            var newChunk = new WorldChunkData(x, z);
            var ents = WorldConstructor.GenerateEntitiesByChunk(x, z, result);

            foreach (var ent in ents)
            {
                AddEntity(ent);
            }
            worldChunkDatas.Add(newChunk);
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

    private void AddEntity(EntityData entityData)
    {
        entityData.Id = GetNewId();
        entityData.SetUpdateAction(AddEntityForUpdate);
        entityDatas.Add(entityData);
        AddEntityForUpdate(entityData.Id);
    }

    public void AddEntityForUpdate(long id)
    {
        if (needUpdates.Contains(id)) return;
        needUpdates.Add(id);
    }

    public void RemoveUpdateId(long id)
    {
        if (needUpdates.Contains(id))
        {
            needUpdates.Remove(id);
        }
    }

    public WorldTileData GetWorldTileData(int x ,int z)
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
    // временное решение об отметке уже сгенерированного чанка
    public int Xpos;
    public int Zpos;

    public WorldChunkData(int x, int z)
    {
        Xpos = x;
        Zpos = z;
    }
}