using System;
using System.Collections.Generic;

public class EntityData
{
    public long Id;
    public List<ComponentData> Components = new();

    private Action<long> _updater;

    public virtual string DebugField => string.Empty;

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

    public void SetUpdateAction(Action<long> updater)
    {
        _updater = updater;
    }

    internal void UpdateEntity()
    {
        _updater?.Invoke(Id);
    }

    public virtual void Touch(int paramTouch = 0)
    {
        
    }
}

public class EntityStartSpawnPoint : EntityData
{
    public EntityStartSpawnPoint(float xpos, float zpos) : base(xpos, zpos)
    {

    }
}

public class EntityMiniMob : EntityData
{
    public EntityMiniMob(float xpos, float zpos) : base(xpos, zpos)
    {

    }
}