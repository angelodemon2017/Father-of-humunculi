using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using static OptimazeExtensions;

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

//    private bool _gameLaunched = false;
    [UnityEngine.SerializeField] private WorldData _gameWorld;

    private readonly object lockObjectEips = new object();
    private Dictionary<long, EntityInProcess> _cashEntities = new();
    private Dictionary<(int, int), Dictionary<long, EntityInProcess>> _cashEIPsByChunk = new();

    private TimeSpan _sessionTime = new();
    private TimeSpan _second = TimeSpan.FromSeconds(1);
    private float _seconder;

    public WorldData GameWorld => _gameWorld;
    public List<EntityInProcess> Entities => _cashEntities.Values.ToList();

    public GameProcess()
    {
//        _entitiesLibrary = Resources.LoadAll<EntitiesLibrary>(string.Empty).FirstOrDefault();
    }

    public List<EntityInProcess> GetEntitiesByChunk(int x, int z)
    {
        lock (lockObjectEips)
        {

            if (_cashEIPsByChunk.TryGetValue((x, z), out Dictionary<long, EntityInProcess> dictEIPs))
            {
                return dictEIPs.Values.ToList();
            }
            else
            {
                return new();
            }
        }
    }

    public void NewGame(WorldData worldData)
    {
//        _gameLaunched = false;
        lock (lockObjectEips)
        {
            _cashEntities.Clear();
        }
        _gameWorld = worldData; 
        CheckEntities();

        StartGame();
    }

    //New game
    public GameProcess(WorldData newWorld)
    {
        _gameWorld = newWorld;
        _gameWorld.GetEnts().ForEach(e => AddEIP(new(e)));
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
        if (!_gameWorld.HaveEnt(commandData.IdEntity))
        {
            return;
        }

        if (commandData.KeyComponent == TypeCache<ComponentPosition>.IdType)
        {
            //TODO update _cashEIPsByChunk
        }

        var ent = _gameWorld.GetEntityById(commandData.IdEntity);

        ent.ApplyCommand(commandData);

        var entConfig = EntitiesLibrary.Instance.GetConfig(ent.TypeKey);
        entConfig.UseCommand(ent, commandData.KeyComponent, commandData.AddingKeyComponent, commandData.KeyCommand, commandData.Message, _gameWorld);
    }

    public void StartGame()
    {
//        _gameLaunched = true;
    }

    public void StopGame()
    {
//        _gameLaunched = false;
    }

    Stopwatch stopwatch = new Stopwatch();
    public void GameTime(float deltaTime)
    {
        //is host
        stopwatch.Reset();
        stopwatch.Start();
        CheckEntities();
        _seconder += deltaTime;
        if (_seconder >= 1f)
        {
            _sessionTime.Add(_second);
            _seconder -= 1f;

//                        SecondEntities();
            Thread thread = new Thread(SecondEntities);
            thread.Start();
        }
        stopwatch.Stop();
        if (WorldViewer.Instance.DebugMode && stopwatch.ElapsedMilliseconds > 0)
        {
            lock (lockObjectEips)
            {
                UnityEngine.Debug.Log($"Entities: {_cashEntities.Count()}, components: {GameplayAdapter.Instance.TESTCOMPONENTS}, total calls:{_cashEntities.Count() * GameplayAdapter.Instance.TESTCOMPONENTS}");
            }
            UnityEngine.Debug.Log("Second update: " + stopwatch.ElapsedMilliseconds + " ms");
        }
    }

    private void SecondEntities()
    {
        lock (lockObjectEips)
        {
            foreach (var entIP in _cashEntities)
            {
                entIP.Value.DoSecond();
            }
        }
    }

    public void CheckEntities()
    {
        //is host
        HashSet<long> idsForDel = new();
        foreach (var newEnt in _gameWorld.GetIds())
        {
            lock (lockObjectEips)
            {
                if (_cashEntities.TryGetValue(newEnt, out EntityInProcess eip))
                {
                    eip.UpdateEntity();

                    if (eip.EntityIsDeleted)
                    {
                        RemoveEIP(eip);
                    }
                }
                else
                {
                    if (_gameWorld.HaveEnt(newEnt))
                    {
                        var ed = _gameWorld.GetEntityById(newEnt);
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
                //TODO this send to network for clients
                idsForDel.Add(newEnt);
            }
        }

        foreach (var id in idsForDel)
        {
            _gameWorld.RemoveUpdateId(id);
        }
    }

    public void AddEIP(EntityInProcess eip)
    {
        foreach (var cmp in eip.EntityData._cashComponents.Values)
        {
            cmp.SetIdEntity(eip.EntityData.Id);
        }

        lock (lockObjectEips)
        {
            _cashEntities.Add(eip.EntityData.Id, eip);

            var tempPos = eip.Position.GetChunkPosInt();
            if (_cashEIPsByChunk.TryGetValue((tempPos.x, tempPos.z), out Dictionary<long, EntityInProcess> dictEIPs))
            {
                dictEIPs.Add(eip.EntityData.Id, eip);
            }
            else
            {
                _cashEIPsByChunk.Add((tempPos.x, tempPos.z), new Dictionary<long, EntityInProcess>());
                _cashEIPsByChunk[(tempPos.x, tempPos.z)].Add(eip.EntityData.Id, eip);
            }
        }
    }

    public void RemoveEIP(EntityInProcess eip)
    {
        lock (lockObjectEips)
        {
            _cashEntities.Remove(eip.Id);

            var tempPos = eip.Position.GetChunkPosInt();
            _cashEIPsByChunk[(tempPos.x, tempPos.z)].Remove(eip.Id);
        }
    }

    private void MessageAboutSpawnEntity(EntityInProcess eip)
    {//is host to client
        WorldViewer.Instance.TryAddEntity(eip);
    }
}