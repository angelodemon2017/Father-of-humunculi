
public class ComponentPlayerId : ComponentData
{
    public string PlayerName;

    private ItemData _itemInHand;// = new ItemData(ItemsController.GetEmpty());

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

//        var posit = CameraController.Instance.FocusPosition;

//        GameProcess.Instance.GameWorld.AddEntity(new EntityItem(item, posit.x, posit.z));

        item.SetEmpty();
    }
    /*    public override void Init(Transform entityME)
        {
            CameraController.Instance.SetTarget(entityME);
            var eip = entityME.GetComponent<EntityMonobeh>();
            UIPlayerManager.Instance.InitEntity(eip);
        }/**/
}