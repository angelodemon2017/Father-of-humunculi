using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ItemsController
{
    private static List<ItemConfig> _items = new();

    public static ItemConfig GetItem(string key)
    {
        Init();

        return _items.FirstOrDefault(i => i.Key == key);
    }

    public static ItemConfig GetItem(EnumItem key)
    {
        Init();

        return _items.FirstOrDefault(i => i.EnumKey == key);
    }

    public static ItemConfig GetEmpty()
    {
        return GetItem(EnumItem.None);
    }

    private static void Init()
    {
        if (_items.Count == 0)
        {
            var tempItems = Resources.LoadAll<ItemConfig>(Config.PathItems).ToList();
            tempItems.ForEach(i => _items.Add(i));
        }
    }
}