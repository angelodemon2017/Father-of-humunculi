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
        var slot = Items.FirstOrDefault(i => i.EnumId == item.EnumId && !i.IsFullSlot);
        if (slot == null)
        {
            slot = Items.FirstOrDefault(i => i.EnumId == EnumItem.None);
        }

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

    public void DropSlot(int index)
    {
        var item = Items[index];
        DropItem(item);
    }

    public void TryApplyRecipe(RecipeSO recipe)
    {
        if (!AvailableRecipe(recipe))
        {
            return;
        }

        foreach (var r in recipe.Resources)
        {
            SubtrackItems(r.ItemConfig.EnumKey, r.Count);
        }

        var resultItem = new ItemData(recipe.Result.ItemConfig);
        resultItem.Count = recipe.Result.Count;
        AddItem(resultItem);
    }

    public bool AvailableRecipe(RecipeSO recipe)
    {
        foreach (var r in recipe.Resources)
        {
            if (GetCountOfItem(r.ItemConfig.EnumKey) < r.Count)
            {
                return false;
            }
        }

        return true;
    }

    public int GetCountOfItem(EnumItem enumItem)
    {
        return Items.Where(i => i.EnumId == enumItem).Sum(i => i.Count);
    }

    private bool SubtrackItems(EnumItem enumItem, int count)
    {
        var slot = Items.FirstOrDefault(i => i.EnumId == enumItem);
        if (slot == null || count == 0 || enumItem == EnumItem.None)
        {
            return false;
        }

        if (slot.Count >= count)
        {
            slot.Count -= count;
            if (slot.Count == 0)
            {
                slot.SetEmpty();
            }
        }
        else
        {
            var dif = count - slot.Count;
            slot.SetEmpty();
            SubtrackItems(enumItem, dif);
        }

        return true;
    }

    public void DropItem(ItemData item)
    {
        if (item.EnumId == EnumItem.None)
        {
            return;
        }

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