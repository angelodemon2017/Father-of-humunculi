using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class WorldData
{
    public string Name;
    public SeedData Seed = new();

    public List<WorldTileData> worldTileDatas = new();
    public List<WorldChunkData> worldChunkDatas = new();//labels about loaded??

    public List<EntityData> entityDatas = new();

    private Dictionary<(int, int), WorldTileData> _cashTiles = new();
    private List<long> _deletedIds = new();

    public long lastIds = 0;
    public long GetNewId () 
    {
        lastIds++;
        return lastIds; 
    }
    public List<long> needUpdates = new();
    public bool IsDeleted(long idCheck)
    {
        return _deletedIds.Any(x => x == idCheck);
    }

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

        AddEntity(EntityController.GetEntity("Player").CreateEntity(0f, 0f));
/*        if (false)
        {//TODO replace to player from SO
            AddEntity(new EntityPlayer(0f, 0f));
        }/**/
    }

    public IEnumerator CheckAndGenChunk(int x, int z)
    {
        yield return new WaitForSeconds(0.1f);
        if (!worldChunkDatas.Any(c => c.Xpos == x && c.Zpos == z))
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
                var xpos = x * Config.ChunkTilesSize + _x - 1;//TODO 1 replace to function
                var zpos = z * Config.ChunkTilesSize + _z - 1;

                var tile = GetWorldTileData(xpos, zpos);

                result.Add(tile);
            }

        if (!worldChunkDatas.Any(c => c.Xpos == x && c.Zpos == z))
        {
            var newChunk = new WorldChunkData(x, z);
            var ents = BiomsController.GetBiom().GenerateEntitiesByChunk(result, Seed);
//                WorldConstructor.GenerateEntitiesByChunk(x, z, result);

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

    public EntityData GetEntityById(long id)
    {
        return entityDatas.FirstOrDefault(e => e.Id == id);
    }

    public void AddEntity(EntityData entityData)
    {
        entityData.Id = GetNewId();
        entityData.SetUpdateAction(AddEntityForUpdate);
        entityDatas.Add(entityData);
        AddEntityForUpdate(entityData.Id);
    }

    public void RemoveEntity(long id)
    {
        var ed = entityDatas.FirstOrDefault(e => e.Id == id);
        if (ed != null)
        {
            entityDatas.Remove(ed);
            needUpdates.Add(id);
            _deletedIds.Add(id);
        }
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
        if (!_cashTiles.TryGetValue((x, z), out WorldTileData tile))
        {
            tile = WorldConstructor.GenerateTile(x, z, Seed, 1);
            worldTileDatas.Add(tile);
            _cashTiles.Add((x, z), tile);
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