using UnityEngine;

public class PlayerPresent : PrefabByComponentData
{
    [SerializeField] private ItemConfig _emptyItem;
    [SerializeField] private EntityMonobeh _entityMonobeh;

    private ComponentPlayerId _component;

    public override string KeyComponent => typeof(PlayerPresent).Name;
    public override string KeyComponentData => typeof(ComponentPlayerId).Name;

    internal override ComponentData GetComponentData => new ComponentPlayerId(new ItemData(_emptyItem));

    public override void Init(ComponentData componentData, EntityInProcess entityInProcess = null)
    {
        _component = (ComponentPlayerId)componentData;

        CameraController.Instance.SetTarget(_entityMonobeh.transform);
        UIPlayerManager.Instance.InitEntity(_entityMonobeh);
    }
}