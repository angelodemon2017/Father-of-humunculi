public class DemoCounter : PrefabByComponentData
{
    public override string KeyComponent => typeof(DemoCounter).Name;
    public override string KeyComponentData => typeof(ComponentCounter).Name;

    public override string GetDebugText => $"res: {_component._debugCounter}";

    private ComponentCounter _component;

    public override void Init(ComponentData componentData, EntityInProcess entityInProcess = null)
    {
        _component = (ComponentCounter)componentData;
    }

}