﻿public class ComponentItemPresent : ComponentData
{
    private ItemData _itemData;

    public ItemConfig ItemConfig => ItemsController.GetItem(_itemData.EnumId);
    public ItemData ItemData => _itemData;

    public string Tip => $"{_itemData.EnumId}({_itemData.Count})";

    public ComponentItemPresent(EnumItem keyItem) : base()
    {
        _itemData = new ItemData(ItemsController.GetItem(keyItem));
    }

    public ComponentItemPresent(ItemData keyItem) : base()
    {
        _itemData = new ItemData(keyItem);
    }
}