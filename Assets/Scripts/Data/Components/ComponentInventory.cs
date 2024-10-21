using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ComponentInventory : ComponentData
{
    public int SomeResource;
    public List<ItemData> Items = new();
    public int MaxItems;

    private Transform _entityME;

    public ComponentInventory(int maxItems = 5)
    {
        MaxItems = maxItems;
        var emptyConf = ItemsController.GetItem(EnumItem.None);
        for (var i = 0; i < maxItems; i++)
        {
            Items.Add(new ItemData(emptyConf));
        }
    }

    public override void Init(Transform entityME)
    {
        _entityME = entityME;
    }

    public void AddItem(ItemData item)
    {
        var slot = Items.FirstOrDefault(i => i.EnumId == item.EnumId && !i.IsFullSlot || i.EnumId == EnumItem.None);
        if (slot == null)
        {
            if (Items.Count >= MaxItems)
            {
                DropItem(item);
                return;
            }

            Items.Add(item);
        }
        else
        {
            if (slot.EnumId == EnumItem.None)
            {
                slot.EnumId = item.EnumId;
            }

            var iConf = ItemsController.GetItem(item.EnumId);

            int freeCountInSlot = iConf.AmountStack - slot.Count;

            if (item.Count > freeCountInSlot)
            {
                int dif = item.Count - freeCountInSlot;
                slot.Count += freeCountInSlot;

                item.Count = dif;
                AddItem(item);
            }
            else
            {
                slot.Count += item.Count;
            }
        }
    }

    public void DropItem(ItemData item)
    {
        GameProcess.Instance.GameWorld.AddEntity(new EntityItem(item, _entityME.position.x, _entityME.position.z));

        item.SetEmpty();
    }

    public void TestLog()
    {
        Debug.Log($"Count slots with items ={Items.Count}");
        foreach (var i in Items)
        {
            var iConf = ItemsController.GetItem(i.EnumId);

            Debug.Log($"{i.EnumId}, count:{i.Count}/{iConf.AmountStack}");
        }
    }

    public void AddSource(int newSource)
    {
        SomeResource += newSource;
    }
}