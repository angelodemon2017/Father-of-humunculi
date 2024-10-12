using System;
using System.Collections.Generic;

public class EntityData
{
    public long Id;
    public List<ComponentData> Components = new();

    private Action<long> _updater;

    public virtual string DebugField
    {
        get
        {
            //            var ent = this as EntityResource;
            return string.Empty;//$"{(ent.IdResource == 0 ? "камень" : "дерево")}";
        }
    }

    public UnityEngine.Vector3 Position 
    {
        get
        {
            var comp = Components.GetComponent<ComponentPosition>();
            return comp == null ? UnityEngine.Vector3.zero : comp.Position;
        }
    }

    public EntityData()
    {

    }

    public void SetUpdateAction(Action<long> updater)
    {
        _updater = updater;
    }

    internal void UpdateEntity()
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
    public int TestValue = 0;

    public override string DebugField => $"{(IdResource == 0 ? "камень" : "дерево")}({TestValue})";

    public EntityResource(int idResource, float xpos, float zpos) : base()
    {
        IdResource = idResource;
        Components.Add(new ComponentPosition(xpos, zpos));
        Components.Add(new ComponentCounter(50, UpperTestValue));
    }

    private void UpperTestValue()
    {
        TestValue++;
        UpdateEntity();
    }
}