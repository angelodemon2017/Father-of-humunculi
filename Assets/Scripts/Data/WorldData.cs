using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class WorldData
{
    public string Name;
    public SeedData Seed = new();

//    public List<WorldTileData> worldTileDatas = new();
    public List<WorldChunkData> worldChunkDatas = new();//labels about loaded?? or replace to structure
//    public List<ResearchEntity> researches = new();

    private Dictionary<string, ResearchEntity> _researches = new();//TODO need replace key to int
    private Dictionary<long, EntityData> _cashEntityDatas = new();

    private Dictionary<(int, int), WorldTileData> _cashTiles = new();
    /// <summary>
    /// Need only for Generation
    /// </summary>
    private Dictionary<(int, int), HashSet<EntityData>> _entityByChunk = new();
    private HashSet<long> _deletedIds = new();

    public long lastIds = 0;
    public long GetNewId () 
    {
        lastIds++;
        return lastIds;
    }

    private readonly object lockObjectUpdateIds = new object();
    private readonly object lockObjectEntities = new object();
    public HashSet<long> needUpdates = new();

    public int CountEntityData => _cashEntityDatas.Count();

    internal HashSet<EntityData> GetEntsByGridChunk(int xChunk, int zChunk, bool skipCenter = true)
    {
        HashSet<EntityData> allNeigs = new();
        for (int x = -1; x < 2; x++)
            for (int z = -1; z < 2; z++)
            {
                if (skipCenter && x == 0 && z == 0)
                {
                    continue;
                }
                var tempEnts = GetEntitiesByChunk(xChunk + x, zChunk + z);
                foreach (var ent in tempEnts)
                {
                    allNeigs.Add(ent);
                }
            }
        return allNeigs;
    }

    internal HashSet<EntityData> GetEntitiesByChunk(int x, int z)
    {
        if (_entityByChunk.TryGetValue((x, z), out HashSet<EntityData> ents))
        {
            return ents;
        }
        return new HashSet<EntityData>();
    }

    public void UpgradeResearch(string name, int val)
    {
        if (_researches.TryGetValue(name, out ResearchEntity researchEntity))
        {
            researchEntity.Progress += val;
        }
        else
        {
            _researches.Add(name, new ResearchEntity(name, val));
        }
    }

    public int GetStatusResearch(string name)
    {
        return _researches.ContainsKey(name) ? _researches[name].Progress : 0;
    }

    public HashSet<long> GetIds()
    {
        lock (lockObjectUpdateIds)
        {
            return new(needUpdates);
        }
    }

    public List<EntityData> GetEnts()
    {
        lock (lockObjectEntities)
        {
            return _cashEntityDatas.Values.ToList();
        }
    }

    public bool IsDeleted(long idCheck)
    {
        return _deletedIds.Contains(idCheck);
    }

    public WorldData()
    {
//        StartGeneration();
    }

    internal void StartGeneration()
    {
        for (int x = -2; x < 3; x++)
            for (int z = -2; z < 3; z++)
            {
                GetChunk(x, z);
            }

        if (true)
        {
            AddEntity(EntitiesLibrary.Instance.GetConfig("Player").CreateEntity(0f, 0f));
        }
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

            var ents = WorldConstructor.GenerateEntitiesByChunk(result, this, 1);
            //BiomsController.GetBiom().GenerateEntitiesByChunk(result, Seed);
            //                WorldConstructor.GenerateEntitiesByChunk(x, z, result);

            foreach (var ent in ents)
            {
                AddEntity(ent, true);
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

    public bool HaveEnt(long id)
    {
        lock (lockObjectEntities)
        {
            return _cashEntityDatas.ContainsKey(id);
        }
    }         

    public EntityData GetEntityById(long id)
    {
        lock (lockObjectEntities)
        {
            return _cashEntityDatas[id];
        }
    }

    public long AddEntity(EntityData entityData, bool needCheckNeigs = false)
    {
        var tempKey = entityData.GetChunk();
        if (needCheckNeigs)
        {
            var neigEnts = GetEntsByGridChunk(tempKey.Item1, tempKey.Item2);
//            Debug.Log($"AddEntity with check needCheckNeigs neigs:{neigEnts.Count()}");
            foreach (var ent in neigEnts)
            {
                if (ent.IsTooClose(entityData))
                {
//                    Debug.Log("Miss add Entity");
                    return -1;
                }
            }
        }

        entityData.Id = GetNewId();
        entityData.SetUpdateAction(AddEntityForUpdate);

        lock (lockObjectEntities)
        {
            _cashEntityDatas.Add(entityData.Id, entityData);

            if (!_entityByChunk.ContainsKey(tempKey))
            {
                _entityByChunk.Add(tempKey, new HashSet<EntityData>());
            }
            _entityByChunk[tempKey].Add(entityData);
        }
        AddEntityForUpdate(entityData.Id);

        return entityData.Id;
    }

    public void RemoveEntity(long id)
    {
        lock (lockObjectEntities)
        {
            var ed = GetEntityById(id);
            if (ed != null)
            {
                _entityByChunk[ed.GetChunk()].Remove(ed);
                _cashEntityDatas.Remove(id);

                lock (lockObjectUpdateIds)
                {
                    needUpdates.Add(id);
                }
                _deletedIds.Add(id);
            }
        }
    }

    public void AddEntityForUpdate(long id)
    {
        lock (lockObjectUpdateIds)
        {
            needUpdates.Add(id);
        }
    }

    public void RemoveUpdateId(long id)
    {
        lock (lockObjectUpdateIds)
        {
            needUpdates.Remove(id);
        }
    }

    public WorldTileData GetWorldTileForMap(int x, int z)
    {
        if (_cashTiles.TryGetValue((x, z), out WorldTileData tile))
        {
            return tile;
        }
        else
        {
            return null;
        }
    }

    public WorldTileData GetWorldTileData(int x ,int z)
    {
        if (!_cashTiles.TryGetValue((x, z), out WorldTileData tile))
        {
            tile = WorldConstructor.GenerateTile(x, z, Seed, 1);
//            worldTileDatas.Add(tile);
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