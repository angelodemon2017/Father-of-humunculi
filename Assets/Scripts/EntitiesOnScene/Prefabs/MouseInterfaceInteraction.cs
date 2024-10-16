public class MouseInterfaceInteraction : PrefabByComponentData
{
    private EntityMonobeh _linkParent;

    public override string KeyComponent => typeof(ComponentInterractable).Name;
    public EntityMonobeh EM => _linkParent;

    public override void Init(ComponentData componentData, EntityInProcess entityInProcess = null)
    {
        _linkParent = GetComponentInParent<EntityMonobeh>();
    }

    public void Click()
    {

    }
}