using static OptimazeExtensions;

public class ComponentUsingByEntity : ComponentData
{
    public long EntityId = -1;

    public ComponentUsingByEntity() : base(TypeCache<ComponentUsingByEntity>.IdType)
    {

    }

    public void SetEntityOpener(long idEnt)
    {
        EntityId = idEnt;
    }
}