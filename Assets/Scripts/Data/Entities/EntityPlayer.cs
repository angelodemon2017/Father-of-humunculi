using System.Collections.Generic;
using UnityEngine;

public class EntityPlayer : EntityData
{
    public override string DebugField => $"asdqwe";

    private ItemData _itemInHand;

    public ItemData ItemHand => _itemInHand;

    public EntityPlayer(float xpos, float zpos) : base(xpos, zpos)
    {
        _itemInHand = new ItemData(ItemsController.GetEmpty());

        Components.AddRange(new List<ComponentData>()
        {
            new ComponentModelPrefab("Player"),
            new ComponentFSM("Player"),//switch to net game
            new ComponentPlayerId(),
            new ComponentInventory(),
            new ComponentUIlabels(Color.white, 1.5f),
        });
    }

    public void PickItemByHand(ItemData item)
    {
        DropItem(_itemInHand);
        _itemInHand.Replace(item);
    }

    private void DropItem(ItemData item)
    {
        if (item.IsEmpty)
        {
            return;
        }

        GameProcess.Instance.GameWorld.AddEntity(new EntityItem(item, Position.x, Position.z));

        item.SetEmpty();
    }

    public override void ApplyCommand(CommandData command)
    {
        base.ApplyCommand(command);
    }
}