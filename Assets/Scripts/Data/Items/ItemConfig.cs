using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemConfig", order = 1)]
public class ItemConfig : ScriptableObject
{
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

    private Dictionary<int, Color> _qualityColors = new()
    {
        { 0, Color.gray },
        { 1, Color.white },
        { 2, Color.green },
        { 3, Color.cyan },
        { 4, Color.magenta },
        { 5, Color.yellow },
        { 6, Color.red },
    };

    public Color ColorBackGround => _qualityColors[BaseQuality];
    public bool IsUseLess => ItemActions.Count > 0;

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