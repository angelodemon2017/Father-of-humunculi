using System;
using System.Collections.Generic;

[Serializable]
public class EntityInProcess
{
    private EntityData _entityData;
    //    private List<ComponentInProcess<ComponentData>> _components = new();
    //    private List<ComponentInProcess<ComponentData>> _updaterComponents = new();
    private List<ISeconder> _updaterComponents = new();

    private EntityMonobeh _config = null;

    public UnityEngine.Vector3 Position => _entityData.Position;
    public long Id => _entityData.Id;
    public string TestDebugProp => _entityData.DebugField;
    public EntityData EntityData => _entityData;

    public Action UpdateEIP;
    public bool EntityIsDeleted => GameProcess.Instance.GameWorld.IsDeleted(Id);
    private EntityMonobeh GetMonobeh
    {
        get
        {
            if (_config == null)
            {
                _config = EntitiesLibrary.Instance.GetConfig(_entityData.TypeKey);
            }

            return _config;
        }
    }

    public EntityInProcess(EntityData entityData)
    {
        _entityData = entityData;
        foreach (var cd in _entityData.Components)
        {
            if (cd is ISeconder cs)
                _updaterComponents.Add(cs);
        }
    }

    public virtual void DoSecond()
    {
        foreach (var componentIP in _updaterComponents)
        {
            componentIP.DoSecond();
        }
        _entityData.DoSecond();
        GetMonobeh.PrefabsByComponents.ForEach(pc => pc.DoSecond(_entityData));
//        _entityData.Config.DoSecond(_entityData);
    }

    public void SendCommand(CommandData command)
    {//TODO place for network sending
        command.IdEntity = Id;
        GameProcess.Instance.SendCommand(command);
//        _entityData.ApplyCommand(command);
    }

    public void UpdateEntity()
    {
        UpdateEIP?.Invoke();
    }
}