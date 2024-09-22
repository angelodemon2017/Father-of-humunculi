using System.Collections.Generic;

public static class Dict
{
    public static Dictionary<EnumFraction, EnumFraction> TargetAttack = new()
    {
        { EnumFraction.player, EnumFraction.enemy },
        { EnumFraction.friend, EnumFraction.enemy },
        { EnumFraction.enemy, EnumFraction.player | EnumFraction.friend },
    };
}