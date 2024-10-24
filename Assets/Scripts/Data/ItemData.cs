using UnityEngine;

public class ItemData
{
    public EnumItem EnumId;
    public string Id;//or string or enum?
    public int Count;
    public int Durability;
    public int Quality;
    public string Meta;

    public bool IsFullSlot => Count == ItemsController.GetItem(EnumId).AmountStack;

    public ItemData(ItemConfig config)
    {
        EnumId = config.EnumKey;
        Id = config.Key;
        Count = Random.Range(config.MinSpawnItem, config.MaxSpawnItem + 1);
        Durability = config.BaseDurability;
        Quality = config.BaseQuality;
        Meta = config.BaseMeta;
    }

    public ItemData(ItemData itemData)
    {
        EnumId = itemData.EnumId;
        Id = itemData.Id;
        Count = itemData.Count;
        Durability = itemData.Durability;
        Quality = itemData.Quality;
        Meta = itemData.Meta;
    }

    public void Replace(ItemData item)
    {
        EnumId = item.EnumId;
        Id = item.Id;
        Count = item.Count;
        Durability = item.Durability;
        Quality = item.Quality;
        Meta = item.Meta;
    }

    public void SetEmpty()
    {
        EnumId = EnumItem.None;
        Count = 0;
    }
}