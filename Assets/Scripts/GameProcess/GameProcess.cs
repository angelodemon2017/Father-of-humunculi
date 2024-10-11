using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameProcess
{
    private static GameProcess _instance;
    public static GameProcess Instance
    {
        get 
        {
            if (_instance == null)
            {
                _instance = new GameProcess();
            }

            return _instance;
        }
    }

    private bool _gameLaunched = false;
    private WorldData _gameWorld;
    private List<EntityInProcess> _entities = new();
    /// <summary>
    /// key - by chunk
    /// </summary>
//    private Dictionary<(int,int),List<EntityInProcess>> _cashEntities = new();

    private TimeSpan _sessionTime = new();
    private TimeSpan _second = TimeSpan.FromSeconds(1);
    private float _seconder;

    public WorldData GameWorld => _gameWorld;
    public List<EntityInProcess> Entities => _entities;

    public GameProcess()
    {
//        NewGame(new WorldData());
    }

    public List<EntityInProcess> GetEntitiesByChunk(int x, int z)
    {
        var centerChunk = Config.ChunkSize / 2;
        var swiftChunk = Config.ChunkTilesSize - Config.TileSize / 2;
        //        Debug.Log($"AllEntities:{_entities.Count()}");
        var xmin = x - centerChunk + swiftChunk;
        var xmax = x + centerChunk + swiftChunk;
        var zmin = z - centerChunk + swiftChunk;
        var zmax = z + centerChunk + swiftChunk;

        //        Vector3 chunkPos = new Vector3(x, 0f, z);
        //        var testVectors = _entities.Select(e => e.Position.GetChunkPos()).ToList();
        //        var result = _entities.Where(x => x.Position.GetChunkPos() == chunkPos).ToList();

        var result = _entities.Where(e => 
            e.Position.x >= xmin &&
            e.Position.x < xmax &&
            e.Position.z >= zmin &&
            e.Position.z < zmax).ToList();

        return result;
    }

    public void NewGame(WorldData worldData)
    {
        _gameLaunched = false;
//        _cashEntities.Clear();
        _entities.Clear();
        _gameWorld = worldData; 
        CheckEntities();

        StartGame();
    }

    //New game
    public GameProcess(WorldData newWorld)
    {
        _gameWorld = newWorld;
        _gameWorld.entityDatas.ForEach(e => _entities.Add(new(e)));
    }

    public void ConnectToHost()
    {
        //request WorldData by host data
    }

    public void GetRequest()
    {
        //send response
    }

    public void StartGame()
    {
        _gameLaunched = true;
    }

    public void StopGame()
    {
        _gameLaunched = false;
    }

    public void GameTime(float deltaTime)
    {
        CheckEntities();
        _seconder += deltaTime;
        if (_seconder >= 1f)
        {
            _sessionTime.Add(_second);
            _seconder -= 1f;
            
/*            foreach (var _cashEnt in _cashEntities) 
            {
                _cashEnt.Value.ForEach(e => e.DoSecond());
            }/**/
            foreach (var entIP in _entities)
            {
                entIP.DoSecond();
            }/**/
        }
    }

    public void CheckEntities()
    {
        HashSet<long> idsForDel = new();
        foreach (var newEnt in _gameWorld.needUpdates)
        {
            if (!_entities.Any(x => x.Id == newEnt))
            {
                var ed = _gameWorld.entityDatas.FirstOrDefault(x => x.Id == newEnt);
                _entities.Add(new EntityInProcess(ed));
            }
            idsForDel.Add(newEnt);
        }

        foreach (var id in idsForDel)
        {
            _gameWorld.RemoveUpdateId(id);
        }
    }
}