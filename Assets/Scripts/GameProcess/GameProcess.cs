using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

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
    [UnityEngine.SerializeField] private WorldData _gameWorld;
    [UnityEngine.SerializeField] private List<EntityInProcess> _entities = new();
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

        var xmin = x - centerChunk;
        var xmax = x + centerChunk;
        var zmin = z - centerChunk;
        var zmax = z + centerChunk;

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
        _gameWorld.entityDatas.ForEach(e => AddEIP(new(e)));
    }

    public void ConnectToHost()
    {
        //request WorldData by host data
    }

    public void SendCommand(CommandData commandData)
    {
        GetRequest(commandData);//web request in future =>
    }

    public void GetRequest(CommandData commandData)//web request in future <=
    {
        var ent = _gameWorld.GetEntityById(commandData.IdEntity);
        ent.ApplyCommand(commandData);
        ent.Config.UseCommand(ent, commandData.KeyCommand, commandData.Message, _gameWorld);
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
        //is host
        CheckEntities();
        _seconder += deltaTime;
        if (_seconder >= 1f)
        {
            _sessionTime.Add(_second);
            _seconder -= 1f;

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            foreach (var entIP in _entities)
            {
                entIP.DoSecond();
            }
            stopwatch.Stop();
            if (WorldViewer.Instance.DebugMode)
            {
                UnityEngine.Debug.Log($"Entities: {_entities.Count()}, components: {GameplayAdapter.Instance.TESTCOMPONENTS}, total calls:{_entities.Count() * GameplayAdapter.Instance.TESTCOMPONENTS}");
                UnityEngine.Debug.Log("Second update: " + stopwatch.ElapsedMilliseconds + " ms");
            }
        }
    }

    public void CheckEntities()
    {
        //is host
        HashSet<long> idsForDel = new();
        foreach (var newEnt in _gameWorld.needUpdates)
        {
            var eip = _entities.FirstOrDefault(e => e.Id == newEnt);
            if (eip == null)
            {
                var ed = _gameWorld.GetEntityById(newEnt);

                if (ed != null)
                {
                    var neweip = new EntityInProcess(ed);
                    AddEIP(neweip);
                    //is client
                    MessageAboutSpawnEntity(neweip);
                }
                else
                {
                    //was deleted
                }
            }
            else
            {
                eip.UpdateEntity();
            }
            //TODO this send to network for clients
            idsForDel.Add(newEnt);
        }

        foreach (var id in idsForDel)
        {
            _gameWorld.RemoveUpdateId(id);
        }
    }

    public void AddEIP(EntityInProcess eip)
    {
        eip.EntityData.Components.ForEach(c => c.SetIdEntity(eip.EntityData.Id));
        _entities.Add(eip);
    }

    public void RemoveEIP(EntityInProcess eip)
    {
        _entities.Remove(eip);
    }

    private void MessageAboutSpawnEntity(EntityInProcess eip)
    {//is host to client
        WorldViewer.Instance.TryAddEntity(eip);
    }
}