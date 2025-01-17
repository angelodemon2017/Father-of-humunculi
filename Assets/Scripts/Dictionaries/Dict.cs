﻿using System.Collections.Generic;
using UnityEngine;

public static class Dict
{
    public static Dictionary<EnumFraction, EnumFraction> TargetAttack = new()
    {
        { EnumFraction.none, EnumFraction.none },
        { EnumFraction.player, EnumFraction.enemy },
        { EnumFraction.friend, EnumFraction.enemy },
        { EnumFraction.enemy, EnumFraction.player | EnumFraction.friend },
    };

    public static Dictionary<(int, int), EnumTileDirect> directs = new()
    {
        { (-1, 0), EnumTileDirect.left },
        { (1, 0), EnumTileDirect.right },
        { (0, 1), EnumTileDirect.up },
        { (0, -1), EnumTileDirect.down },
        { (-1, -1), EnumTileDirect.ld },
        { (-1, 1), EnumTileDirect.lu },
        { (1, -1), EnumTileDirect.rd },
        { (1, 1), EnumTileDirect.ru },
    };

    public static EnumTileDirect GetDirectBySwift(int X, int Z)
    {
        if (X < -1 || X > 1 || Z < -1 || Z > 1)
        {
            UnityEngine.Debug.Log($"GetDirectBySwift:{X},{Z}");
        }
        return directs[(X, Z)];
    }

    public static class Commands
    {
        public const string UseItem = "UseItem";
        public const string DropItem = "DropItem";
        public const string UseRecipe = "UseRecipe";
        public const string CloseUI = "CloseUI";
        public const string SlotClick = "SlotClick";
        public const string SlotDrag = "SlotDrag";
        public const string SlotDrop = "SlotDrop";
        public const string SplitSlot = "SplitSlot";
        public const string SelectFollow = "SelectFollow";
        public const string SelectRole = "SelectRole";
        public const string SetterState = "SetterState";
        public const string MakeDamage = "MakeDamage";
    }

    public static class RectKeys
    {
        public static string Shop = "Shop";
        public static string GoldBush = "GoldBush";
        public static string DemoFabric = "DemoFabric";
    }

    public static class RecipeGroups
    {
        public static string ShopDebug = "ShopDebug";
    }

    public static class SpecComponents
    {
        public const string ItemPresent = "ItemPresent";
    }
}