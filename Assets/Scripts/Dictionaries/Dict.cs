using System.Collections.Generic;

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
        return directs[(X, Z)];
    }

    public static class Commands
    {
        public const string UseItem = "UseItem";
        public const string DropItem = "DropItem";
        public const string SetEntity = "SetEntity";
        public const string CloseUI = "CloseUI";
        public const string SlotClick = "SlotClick";
        public const string SlotDrag = "SlotDrag";
        public const string SlotDrop = "SlotDrop";
        public const string SplitSlot = "SplitSlot";
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
}