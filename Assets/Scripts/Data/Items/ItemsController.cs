using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ItemsController
{
    private static List<ItemConfig> _items = new();

    public static ItemConfig GetItem(EnumItem key)
    {
        if (_items.Count == 0)
        {
            var tempItems = Resources.LoadAll<ItemConfig>(Config.PathItems).ToList();
            tempItems.ForEach(i => _items.Add(i));
        }

        return _items.FirstOrDefault(i => i.EnumKey == key);
    }
}