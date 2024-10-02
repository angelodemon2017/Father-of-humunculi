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
            var cip = new ComponentInProcess<ComponentData>();
            cip.Init(cd);
            _components.Add(cip);
        }
    }
}