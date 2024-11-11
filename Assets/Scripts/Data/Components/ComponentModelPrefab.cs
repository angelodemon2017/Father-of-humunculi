public class ComponentModelPrefab : ComponentData
{
    public string KeyModel;

    public int CurrentParamOfModel;

    public ComponentModelPrefab()
    {

    }

    public ComponentModelPrefab(string keyModel) : base()
    {
        KeyModel = keyModel;
    }
}