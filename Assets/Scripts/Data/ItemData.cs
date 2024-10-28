using UnityEngine;

public class ItemData
{
    public EnumItem EnumId;
    public string Id;//or string or enum?
    public int Count;
    public int Durability;
    public int Quality;
    public string Meta;

    public bool IsFullSlot => Count == ItemConfig.AmountStack;
    public bool IsEmpty => EnumId == EnumItem.None;
    public ItemConfig ItemConfig => ItemsController.GetItem(EnumId);

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

    public int TryAdd(ItemData item)
    {
        var iConf = item.ItemConfig;
        int freeCountInSlot = iConf.AmountStack - Count;

        if (item.Count > freeCountInSlot)
        {
            int dif = item.Count - freeCountInSlot;
            Count += freeCountInSlot;
            return dif;
        }
        else
        {
            Count += item.Count;
            return 0;
        }
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
        var emptyItem = ItemsController.GetEmpty();

        EnumId = emptyItem.EnumKey;
        Count = 0;
        Id = emptyItem.Key;
        Durability = emptyItem.BaseDurability;
        Quality = emptyItem.BaseQuality;
        Meta = emptyItem.BaseMeta;
    }

    public void SubtractCount(int count = 1)
    {
        Count -= count;
        if (Count <= 0)
        {
            SetEmpty();
        }
    }

    public void UseItem(EntityData entityData)
    {
        ItemConfig.UseItem(this, entityData);
    }
}