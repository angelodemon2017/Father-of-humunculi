using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;
using static OptimazeExtensions;

[Serializable]
public class ComponentInventory : ComponentData
{
    public List<ItemData> Items = new();
    public int MaxItems;

    public ComponentInventory(ComponentInventory component) : this (component.MaxItems) { }

    public ComponentInventory(List<EnumItemCategory> slots, int addingKey) : base(TypeCache<ComponentInventory>.IdType)
    {
        AddingKey = addingKey;
        MaxItems = slots.Count;
        var emptyConf = ItemsController.GetEmpty();
        for (var i = 0; i < MaxItems; i++)
        {
            var newI = new ItemData(emptyConf);
            newI.CATEGORYOFSLOT = slots[i];
            Items.Add(newI);
        }
    }

    public ComponentInventory(int maxItems = 5) : base(TypeCache<ComponentInventory>.IdType)
    {
        MaxItems = maxItems;
        var emptyConf = ItemsController.GetEmpty();
        for (var i = 0; i < maxItems; i++)
        {
            Items.Add(new ItemData(emptyConf));
        }
    }

    public ItemData AddItem(ItemData item)
    {
        if (item == null || item.Count == 0 || item.IsEmpty)
        {
            return item;
        }

        var slot = Items.FirstOrDefault(i => i.Id == item.Id && !i.IsFullSlot && i.AvailableSlotForItem(item));
        if (slot == null)
        {
            slot = Items.FirstOrDefault(i => i.IsEmpty && i.AvailableSlotForItem(item));
        }

        if (slot == null)
        {
            if (Items.Count >= MaxItems)
            {
//                DropItem(item);
                return item;
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
        item.SetEmpty();
        return item;
    }

    public bool AvailableAddItem(ItemData item)
    {
        if (Items.Any(i => i.IsEmpty && i.AvailableSlotForItem(item)))
        {
            return true;
        }
        if (Items.Any(i => i.Id == item.Id && !i.IsFullSlot && i.AvailableSlotForItem(item)))
        {
            return true;
        }

        return false;
    }

    public ItemData AddItem(ItemData item, int idSlot)
    {
        if (item == null || item.Count == 0 || item.IsEmpty)
        {
            return item;
        }

        ItemData itemSlot;
        if (idSlot > 0 || idSlot < Items.Count)
        {
            itemSlot = Items[idSlot];
        }
        else
        {
            if (idSlot == -1)
            {
                var tryadditem = AddItem(item);
                item.SetEmpty();
                return tryadditem;
            }
            return item;
        }

        if (!itemSlot.AvailableSlotForItem(item))
        {
            return item;
        }

        if (itemSlot.IsEmpty)
        {
            itemSlot.Replace(item);
            item.Count -= itemSlot.Count;
        }
        else if (itemSlot.Id == item.Id)
        {
            item.Count = itemSlot.TryAdd(item);
        }
        else
        {
            var tempItem = new ItemData(itemSlot);
            itemSlot.Replace(item);
            item.Replace(tempItem);
        }

        return item;
    }

/*    public void TryApplyRecipe(RecipeItem recipe)
    {
        if (!AvailableRecipe(recipe))
        {
            return;
        }

        SubtrackItemsByRecipe(recipe);

        var resultItem = new ItemData(recipe.ItemResult.ItemConfig);
        resultItem.Count = recipe.ItemResult.Count;
        AddItem(resultItem);
    }/**/

/*    public void SubtrackItemsByRecipe(RecipeSO recipe)
    {
        foreach (var r in recipe.Resources)
        {
            SubtrackItems(r.ItemConfig.Key, r.Count);
        }
    }/**/

    /// <summary>
    /// TODO Need remove method
    /// </summary>
    /// <param name="recipe"></param>
    /// <returns></returns>
/*    public bool AvailableRecipe(RecipeSO recipe)
    {
        foreach (var r in recipe.Resources)
        {
            if (GetCountOfItem(r.ItemConfig.Key) < r.Count)
            {
                return false;
            }
        }

        return true;
    }/**/

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
            //TODO was replace id string to int
            //need KeyCommand, addingkey...
//            KeyComponent = Dict.Commands.UseItem,
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