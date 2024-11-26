using System;
using System.Collections.Generic;

public class EntityData
{
    public long Id;
    public string TypeKey;
    
    public List<ComponentData> Components = new();

    private Action<long> _updater;
    internal WorldData worldData => GameProcess.Instance.GameWorld;
    internal string _DebugField = string.Empty;
    public virtual string DebugField => _DebugField;

    public UnityEngine.Vector3 Position 
    {
        get
        {
            var comp = Components.GetComponent<ComponentPosition>();
            return comp == null ? UnityEngine.Vector3.zero : comp.Position;
        }
    }

    public EntityData(float xpos = 0, float zpos = 0)
    {
        Components.Add(new ComponentPosition(xpos, zpos));
    }

/*    public void DoSecond()
    {
        bool isChanged = false;
        foreach (var c in Components)
        {
            if (c.DoSecond())
            {
                isChanged = true;
            }
        }
        if (isChanged)
        {
            UpdateEntity();
        }
    }/**/

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
        if (command.KeyComponent == typeof(ComponentPosition).Name)
        {
            var comp = Components.GetComponent<ComponentPosition>();
            comp.UpdateByCommand(command.Message);
            UpdateEntity();
        }
    }
}

public class EntityStartSpawnPoint : EntityData
{
    public EntityStartSpawnPoint(float xpos, float zpos) : base(xpos, zpos)
    {

    }
}

public class EntityCapacity : EntityData
{
    public EntityCapacity(int countComponents) : base()
    {
        for (int b = 0; b < countComponents; b++)
        {
            Components.Add(new ComponentCounter(100));
        }
    }
}