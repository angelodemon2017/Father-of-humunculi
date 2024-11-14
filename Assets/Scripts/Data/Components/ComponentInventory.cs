using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

[Serializable]
public class ComponentInventory : ComponentData
{
    public List<ItemData> Items = new();
    public int MaxItems;
    public string AddingKey;

    public ComponentInventory(ComponentInventory component) : this (component.MaxItems) { }

    public ComponentInventory(List<EnumItemCategory> slots)
    {
        MaxItems = slots.Count;
        var emptyConf = ItemsController.GetEmpty();
        for (var i = 0; i < MaxItems; i++)
        {
            var newI = new ItemData(emptyConf);
            newI.CATEGORYOFSLOT = slots[i];
            Items.Add(newI);
        }
    }

    public ComponentInventory(int maxItems = 5)
    {
        MaxItems = maxItems;
        var emptyConf = ItemsController.GetEmpty();
        for (var i = 0; i < maxItems; i++)
        {
            Items.Add(new ItemData(emptyConf));
        }
    }

    public void AddItem(ItemData item)
    {
        if (item == null || item.Count == 0 || item.IsEmpty)
        {
            return;
        }

        var slot = Items.FirstOrDefault(i => i.Id == item.Id && !i.IsFullSlot);
        if (slot == null)
        {
            slot = Items.FirstOrDefault(i => i.IsEmpty);
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
            if (slot.IsEmpty)
            {
                slot.Replace(item);
                slot.Count = 0;
            }

            item.Count = slot.TryAdd(item);
            AddItem(item);
        }
    }

    public void TryApplyRecipe(RecipeSO recipe)
    {
        if (!AvailableRecipe(recipe))
        {
            return;
        }

        SubtrackItemsByRecipe(recipe);

        var resultItem = new ItemData(recipe.Result.ItemConfig);
        resultItem.Count = recipe.Result.Count;
        AddItem(resultItem);
    }

    public void SubtrackItemsByRecipe(RecipeSO recipe)
    {
        foreach (var r in recipe.Resources)
        {
            SubtrackItems(r.ItemConfig.Key, r.Count);
        }
    }

    public bool AvailableRecipe(RecipeSO recipe)
    {
        foreach (var r in recipe.Resources)
        {
            if (GetCountOfItem(r.ItemConfig.Key) < r.Count)
            {
                return false;
            }
        }

        return true;
    }

    public int GetCountOfItem(string keyItem)
    {
        return Items.Where(i => i.Id == keyItem).Sum(i => i.Count);
    }

    public bool SubtrackItems(string keyItem, int count)
    {
        var slot = Items.FirstOrDefault(i => i.Id == keyItem);
        if (slot == null || count <= 0 || keyItem == "none")
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
            SubtrackItems(keyItem, dif);
        }

        return true;
    }

    public void SplitSlot(int index)
    {
        var item = Items[index];
        if (item.IsEmpty || item.Count < 2)
        {
            return;
        }
        var emptySlot = Items.FirstOrDefault(i => i.IsEmpty);
        if (emptySlot == null)
        {
            return;
        }

        var splitCount = item.Count / 2;
        item.Count -= splitCount;

        emptySlot.Replace(item);
        emptySlot.Count = splitCount;
    }

    public void DropSlot(int index)
    {
        var item = Items[index];
        DropItem(item);
    }

    /// <summary>
    ///TODO move to inventorySO
    /// </summary>
    /// <param name="item"></param>
    public void DropItem(ItemData item)
    {//TODO need command 
        if (item.IsEmpty)
        {
            return;
        }

        var ent = GameProcess.Instance.GameWorld.entityDatas.FirstOrDefault(e => e.Id == _idEntity);

        GameProcess.Instance.GameWorld.AddEntity(new EntityItem(item, ent.Position.x, ent.Position.z));

        item.SetEmpty();
    }

    public void TestLog()
    {
        Debug.Log($"Count slots with items ={Items.Count}");
        foreach (var i in Items)
        {
            var iConf = i.ItemConfig;

            Debug.Log($"{i.Id}, count:{i.Count}/{iConf.AmountStack}");
        }
    }

    public static CommandData GetCommandUseItem(int idSlot)
    {//TODO command for split, drop
        return new CommandData()
        {
            KeyComponent = Dict.Commands.UseItem,
            Message = $"{idSlot}",
        };
    }

    public void UseItem(int slot, EntityData entityData)
    {
        if (slot >= Items.Count)
        {
            return;
        }

        Items[slot].UseItem(entityData);
    }
}