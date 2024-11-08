﻿public class ComponentItemPresent : ComponentData
{
    private ItemData _itemData;

    public ItemConfig ItemConfig => _itemData.ItemConfig;
    public ItemData ItemData => _itemData;

    public string Tip => $"{_itemData.EnumId}({_itemData.Count})";

    public ComponentItemPresent()
    {

    }

    public ComponentItemPresent(ItemData keyItem) : base()
    {
        _itemData = new ItemData(keyItem);
    }

    public void SetItem(ItemData keyItem)
    {
        _itemData = new ItemData(keyItem);
    }
}