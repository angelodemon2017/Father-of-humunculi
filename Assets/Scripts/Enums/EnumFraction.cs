﻿[System.Flags]
public enum EnumFraction
{
    none =          0b_0000,
    player =        0b_0000_0000_0000_0000_0000_0000_0000_0001,
    friend =        0b_0000_0000_0000_0000_0000_0000_0000_0010,
    enemy =         0b_0000_0000_0000_0000_0000_0000_0000_0100,
    pickupItem =    0b_0000_0000_0000_0000_0000_0000_0000_1000,
    wildInteract =  0b_0000_0000_0000_0000_0000_0000_0001_0000,
    interact =      0b_0000_0000_0000_0000_0000_0000_0010_0000,
    /*        fraction07 = 0b_0000_0000_0000_0000_0000_0000_0100_0000,
            fraction08 = 0b_0000_0000_0000_0000_0000_0000_1000_0000,
            fraction09 = 0b_0000_0000_0000_0000_0000_0001_0000_0000,
            fraction10 = 0b_0000_0000_0000_0000_0000_0010_0000_0000,
            fraction11 = 0b_0000_0000_0000_0000_0000_0100_0000_0000,
            fraction12 = 0b_0000_0000_0000_0000_0000_1000_0000_0000,
            fraction13 = 0b_0000_0000_0000_0000_0001_0000_0000_0000,
            fraction14 = 0b_0000_0000_0000_0000_0010_0000_0000_0000,
            fraction15 = 0b_0000_0000_0000_0000_0100_0000_0000_0000,
            fraction16 = 0b_0000_0000_0000_0000_1000_0000_0000_0000,
            fraction17 = 0b_0000_0000_0000_0001_0000_0000_0000_0000,
            fraction18 = 0b_0000_0000_0000_0010_0000_0000_0000_0000,
            fraction19 = 0b_0000_0000_0000_0100_0000_0000_0000_0000,
            fraction20 = 0b_0000_0000_0000_1000_0000_0000_0000_0000,
            fraction21 = 0b_0000_0000_0001_0000_0000_0000_0000_0000,
            fraction22 = 0b_0000_0000_0010_0000_0000_0000_0000_0000,
            fraction23 = 0b_0000_0000_0100_0000_0000_0000_0000_0000,
            fraction24 = 0b_0000_0000_1000_0000_0000_0000_0000_0000,
            fraction25 = 0b_0000_0001_0000_0000_0000_0000_0000_0000,
            fraction26 = 0b_0000_0010_0000_0000_0000_0000_0000_0000,
            fraction27 = 0b_0000_0100_0000_0000_0000_0000_0000_0000,
            fraction28 = 0b_0000_1000_0000_0000_0000_0000_0000_0000,
            fraction29 = 0b_0001_0000_0000_0000_0000_0000_0000_0000,
            fraction30 = 0b_0010_0000_0000_0000_0000_0000_0000_0000,
            fraction31 = 0b_0100_0000_0000_0000_0000_0000_0000_0000,/**/
}