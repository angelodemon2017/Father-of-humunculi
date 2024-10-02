using System;
using System.Collections.Generic;

public class GameProcess
{
    private WorldData _gameWorld;
    private List<EntityInProcess> entities = new();

    private TimeSpan _sessionTime = new();
    private TimeSpan _second = TimeSpan.FromSeconds(1);
    private float _seconder;

    //New game
    public GameProcess(WorldData newWorld)
    {
        _gameWorld = newWorld;
        _gameWorld.entityDatas.ForEach(e => entities.Add(new(e)));
    }

    public void GameTime(float deltaTime)
    {
        _seconder += deltaTime;
        if (_seconder > 1f)
        {
            _sessionTime.Add(_second);
            _seconder = 0;
        }
    }
}