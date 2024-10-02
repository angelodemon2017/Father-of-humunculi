using System.Collections.Generic;

public class EntityInProcess
{
    private EntityData _entityData;
    private List<ComponentInProcess<ComponentData>> _components = new();

    public EntityInProcess(EntityData entityData)
    {
        _entityData = entityData;
        foreach (var cd in _entityData.Components)
        {
            _components.Add(new ComponentInProcess<ComponentData>(cd));
        }
    }

    public virtual void DoSecond()
    {
        foreach (var componentIP in _components)
        {
            componentIP.DoSecond();
        }
    }
}