using UnityEngine;

[CreateAssetMenu(menuName = "Components/Player Component", order = 1)]
public class ComponentSOPlayer : ComponentSO
{
    [SerializeField] private ItemConfig _emptyHand;
    [SerializeField] private ComponentPlayerId _componentData;

    private ItemData _emptyItem => new ItemData(_emptyHand);

    internal override ComponentData GetComponentData => new ComponentPlayerId(_emptyItem);

    public override void InitOnScene(EntityMonobeh entityMonobeh)
    {
        CameraController.Instance.SetTarget(entityMonobeh.transform);
        UIPlayerManager.Instance.InitEntity(entityMonobeh);
    }
}