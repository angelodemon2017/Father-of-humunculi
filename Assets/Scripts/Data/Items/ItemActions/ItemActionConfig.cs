using UnityEngine;

public class ItemActionConfig : ScriptableObject
{
    public virtual bool AvailableUseItem(ItemData itemData, EntityData entityData)
    {
        return true;
    }

    public virtual void ApplyItem(ItemData itemData, EntityData entityData)
    {

    }
}