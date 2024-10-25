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
        public static string UseItem = "UseItem";
    }

    public static class RectKeys
    {
        public static string Shop = "Shop";
    }
}