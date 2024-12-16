using System;
using System.Collections.Generic;
using static OptimazeExtensions;

public class EntityData
{
    public long Id;
    public string TypeKey;

    private Action<long> _updater;
    internal WorldData worldData => GameProcess.Instance.GameWorld;
    internal string _DebugField = string.Empty;
    public virtual string DebugField => _DebugField;
    public EntityMonobeh GetConfig => EntitiesLibrary.Instance.GetConfig(TypeKey);

    internal Dictionary<(int, int), ComponentData> _cashComponents = new();

    internal List<T> GetComponents<T>() where T : ComponentData
    {
        int idType = TypeCache<T>.IdType;
        List<T> cmps = new();
        foreach (var cmp in _cashComponents)
        {
            if (cmp.Value.KeyType == idType)
            {
                cmps.Add(cmp.Value as T);
            }
        }

        return cmps;
    }

    internal T GetComponent<T>(int addingKey = 0) where T : ComponentData
    {
        int idT = TypeCache<T>.IdType;
        if (_cashComponents.TryGetValue((idT, addingKey), out ComponentData cmp))
        {
            return cmp as T;
        }
        return null;
    }

    public (int, int) GetChunk()
    {
        var pos = Position.GetChunkPosInt();
        return (pos.x, pos.z);
    }

    public UnityEngine.Vector3 Position 
    {
        get
        {
            var comp = GetComponent<ComponentPosition>();
            return comp == null ? UnityEngine.Vector3.zero : comp.Position;
        }
    }

    public EntityData(float xpos = 0, float zpos = 0)
    {
        var cmpPos = new ComponentPosition(xpos, zpos);
        _cashComponents.Add((cmpPos.KeyType, cmpPos.AddingKey), cmpPos);
    }

    public void SetUpdateAction(Action<long> updater)
    {
        _updater = updater;
    }

    internal void UpdateEntity()
    {
        _updater?.Invoke(Id);
    }

    public virtual void ApplyCommand(CommandData command)
    {
        if (command.KeyComponent == TypeCache<ComponentPosition>.IdType)
        {
            var comp = GetComponent<ComponentPosition>();
            comp.UpdateByCommand(command.Message);
            UpdateEntity();
        }
    }

    internal bool IsTooClose(EntityData checkerEnt)
    {
        return UnityEngine.Vector3.Distance(Position, checkerEnt.Position) > GetConfig.RadEntity + checkerEnt.GetConfig.RadEntity;
    }
}

public class EntityCapacity : EntityData
{
    public EntityCapacity(int countComponents) : base()
    {
        for (int b = 0; b < countComponents; b++)
        {
            var cmpCntr = new ComponentCounter(100);
            _cashComponents.Add((cmpCntr.KeyType, b), cmpCntr);
        }
    }
}