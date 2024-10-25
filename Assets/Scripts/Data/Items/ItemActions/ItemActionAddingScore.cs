using UnityEngine;

[CreateAssetMenu(menuName = "ItemAction/AddingScore", order = 1)]
public class ItemActionAddingScore : ItemActionConfig
{
    public int Score;

    public override bool AvailableUseItem(ItemData itemData, EntityData entityData)
    {
        return entityData is EntityPlayer && itemData.Count > 0;
    }

    public override void ApplyItem(ItemData itemData, EntityData entityData)
    {
        if (entityData is EntityPlayer entityPlayer)
        {
            entityPlayer.ScoreDebug += Score;
            itemData.SubtractCount();
        }
    }
}