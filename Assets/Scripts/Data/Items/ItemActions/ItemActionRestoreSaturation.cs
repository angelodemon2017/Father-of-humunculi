using UnityEngine;

[CreateAssetMenu(menuName = "ItemAction/ItemActionRestoreSaturation", order = 1)]
public class ItemActionRestoreSaturation : ItemActionConfig
{
    [SerializeField] private int Saturation;
    [SerializeField] private int Starvation;
    [SerializeField] private int Gorging;

    public override bool AvailableUseItem(ItemData itemData, EntityData entityData)
    {
        var hung = entityData.GetComponent<ComponentHunger>();
        return hung != null;
    }

    public override void ApplyItem(ItemData itemData, EntityData entityData)
    {
        var hungCmp = entityData.GetComponent<ComponentHunger>();
        hungCmp.RestoreHunger(Starvation, Saturation, itemData.Id, Gorging);

        itemData.Count--;
        if (itemData.Count <= 0)
        {
            itemData.SetEmpty();
        }

        entityData.UpdateEntity();
    }
}