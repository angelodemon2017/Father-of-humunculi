using System;
using System.Collections.Generic;

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
    private List<EntityInProcess> entities = new();

    private TimeSpan _sessionTime = new();
    private TimeSpan _second = TimeSpan.FromSeconds(1);
    private float _seconder;

    public WorldData GameWorld => _gameWorld;

    public GameProcess()
    {
        NewGame(new WorldData());
    }

    public void NewGame(WorldData worldData)
    {
        _gameLaunched = false;
        entities.Clear();
        _gameWorld = worldData;
        StartGame();
    }

    //New game
    public GameProcess(WorldData newWorld)
    {
        _gameWorld = newWorld;
        _gameWorld.entityDatas.ForEach(e => entities.Add(new(e)));
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
        _seconder += deltaTime;
        if (_seconder >= 1f)
        {
            _sessionTime.Add(_second);
            _seconder -= 1f;
            foreach (var entIP in entities)
            {
                entIP.DoSecond();
            }
        }
    }
}