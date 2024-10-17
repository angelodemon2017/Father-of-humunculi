using System;

public class ComponentModelPrefab : ComponentData
{
    public string KeyModel;

    public int CurrentParamOfModel;

    public ComponentModelPrefab(string keyModel) : base()
    {
        KeyModel = keyModel;
    }
}