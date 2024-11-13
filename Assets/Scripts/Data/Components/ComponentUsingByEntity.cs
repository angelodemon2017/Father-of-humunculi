public class ComponentUsingByEntity : ComponentData
{
    public long EntityId = -1;

    public ComponentUsingByEntity()
    {

    }

    public void SetEntityOpener(long idEnt)
    {
        EntityId = idEnt;
    }
}