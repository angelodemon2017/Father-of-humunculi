using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemConfig", order = 1)]
public class ItemConfig : ScriptableObject
{
    public EnumItem EnumKey;
    public string Key;//localize key, dictionary key, etc...
    public string Description;
    public int AmountStack;
    public int BaseDurability;//max durability
    public Sprite IconItem;
    public int BaseQuality;
    public string BaseMeta;
    public int MinSpawnItem;
    public int MaxSpawnItem;
    public List<ItemActionConfig> ItemActions = new();

    public Color ColorBackGround => Color.white;//todo get color from quality

    public bool UseItem(ItemData itemData, EntityData entityData)
    {
        foreach (var ia in ItemActions)
        {
            if (!ia.AvailableUseItem(itemData, entityData))
            {
                return false;
            }
        }

        foreach (var ia in ItemActions)
        {
            ia.ApplyItem(itemData, entityData);
        }

        return true;
    }
}