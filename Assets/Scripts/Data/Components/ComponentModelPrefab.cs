using static OptimazeExtensions;

public class ComponentModelPrefab : ComponentData
{
    public string KeyModel;

    public int CurrentParamOfModel;

    public ComponentModelPrefab() : base(TypeCache<ComponentModelPrefab>.IdType)
    {

    }
}