﻿using System.Collections.Generic;
using UnityEngine;

public class EntityPlayer : EntityData
{
    public override string DebugField => _itemInHand.EnumId == EnumItem.None ? "" : $"In hand {_itemInHand.EnumId}({_itemInHand.Count})";

    private ItemData _itemInHand;

    public ItemData ItemHand => _itemInHand;

    public EntityPlayer(float xpos, float zpos) : base(xpos, zpos)
    {
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
        if (item.EnumId == EnumItem.None)
        {
            return;
        }

        GameProcess.Instance.GameWorld.AddEntity(new EntityItem(item, Position.x, Position.z));

        item.SetEmpty();
    }
}