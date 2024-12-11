using System;

[Serializable]
public class EntityInProcess
{
    private EntityData _entityData;

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
    }

    public virtual void DoSecond()
    {
        GetMonobeh.PrefabsByComponents.ForEach(pc => pc.DoSecond(_entityData));
    }

    public void SendCommand(CommandData command)
    {//TODO place for network sending
        command.IdEntity = Id;
        GameProcess.Instance.SendCommand(command);
    }

    public void UpdateEntity()
    {
        UpdateEIP?.Invoke();
    }
}