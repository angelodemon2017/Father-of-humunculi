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
    }

    public override void Init(Transform entityME)
    {
        _entityME = entityME;
    }

    public void AddItem(ItemData item)
    {
        if (Items.Count >= MaxItems)
        {
            GameProcess.Instance.GameWorld.AddEntity(new EntityItem(item, _entityME.position.x, _entityME.position.z));
            return;
        }

        var slot = Items.FirstOrDefault(i => i.EnumId == item.EnumId && !i.IsFullSlot);
        if (slot == null)
        {
            Items.Add(item);
        }
        else
        {
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