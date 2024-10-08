using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class WorldData
{
    public string Name;
    public string Seed;

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

        if (worldChunkDatas.Any(c => c.Xpos == x && c.Zpos == z))
        {
            worldChunkDatas.Add(GenerateEntitiesChunk(x, z));
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

    const int minimumEntitiesByChunk = 3;
    private WorldChunkData GenerateEntitiesChunk(int x, int z)
    {
        List<Vector3Int> tempPoint = new List<Vector3Int>();
        for (int _x = 0; _x < Config.ChunkTilesSize; _x++)
            for (int _z = 0; _z < Config.ChunkTilesSize; _z++)
            {
                tempPoint.Add(new Vector3Int(_x, 0, _z));
            }
        var pointForGen = new List<Vector3Int>();
        for (int p = 0; p < minimumEntitiesByChunk; p++)
        {
            var tp = tempPoint.GetRandom();
            pointForGen.Add(tp);
            tempPoint.Remove(tp);
        }
        foreach (var tp in pointForGen)
        {
            var xpos = tp.x + x * Config.ChunkTilesSize;
            var zpos = tp.z + z * Config.ChunkTilesSize;
            var ed = GetWorldTileData(xpos, zpos);
            if (ed.Id < 2)
            {
                AddEntity(new EntityResource(0, xpos, zpos));
            }
            else
            {
                AddEntity(new EntityResource(1, xpos, zpos));
            }
        }

        return new WorldChunkData(x, z);
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

    public void RemoveEntity(long id)
    {
        if (needUpdates.Contains(id))
        {
            needUpdates.Remove(id);
        }
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