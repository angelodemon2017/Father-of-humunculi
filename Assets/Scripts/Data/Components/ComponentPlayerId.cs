
public class ComponentPlayerId : ComponentData
{
    public string PlayerName;

    private ItemData _itemInHand;

    public ItemData ItemHand => _itemInHand;

    public ComponentPlayerId(ItemData handItem)
    {
        _itemInHand = handItem;
    }


    public void PickItemByHand(ItemData item)
    {
        DropItem(_itemInHand);//А точно ли нужно такое делать?
        _itemInHand.Replace(item);
    }

    private void DropItem(ItemData item)
    {
        if (item.IsEmpty)
        {
            return;
        }

        item.SetEmpty();
    }
}