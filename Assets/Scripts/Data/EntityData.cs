using System;
using System.Collections.Generic;

public class EntityData
{
    public long Id;
    public List<ComponentData> Components;

    private Action<long> _updater;

    public EntityData()
    {

    }

    public void SetUpdateAction(Action<long> updater)
    {
        _updater = updater;
    }

    public void UpdateEntity()
    {
        _updater?.Invoke(Id);
    }
}

public class EntityStartSpawnPoint : EntityData
{

}

public class EntityResource : EntityData
{
    public int IdResource;

    public EntityResource(int idResource, float xpos, float zpos) : base()
    {
        IdResource = idResource;
        Components.Add(new ComponentPosition(xpos, zpos));

    }
}