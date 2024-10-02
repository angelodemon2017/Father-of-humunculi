[System.Flags]
public enum EnumTileDirect : byte
{
    none =  0b_0000_0000,
    left =  0b_1000_0000,
    up =    0b_0100_0000,
    right = 0b_0010_0000,
    down =  0b_0001_0000,
    lu =    0b_0000_1000,
    ld =    0b_0000_0100,
    ru =    0b_0000_0010,
    rd =    0b_0000_0001,
}