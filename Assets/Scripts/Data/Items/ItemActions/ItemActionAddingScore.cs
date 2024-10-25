using UnityEngine;

[CreateAssetMenu(menuName = "ItemAction/AddingScore", order = 1)]
public class ItemActionAddingScore : ItemActionConfig
{
    public int Score;

    public override bool AvailableUseItem(ItemData itemData, EntityData entityData)
    {
        return true;
    }

    public override void ApplyItem(ItemData itemData, EntityData entityData)
    {
        if (entityData is EntityPlayer entityPlayer)
        {

        }
    }
}