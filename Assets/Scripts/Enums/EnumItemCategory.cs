/// <summary>
/// Need to slots
/// </summary>
[System.Flags]
public enum EnumItemCategory : byte
{
    none = 0b_0000,
    trash = 0b_0000_0001,
    resource = 0b_0000_0010,
    head = 0b_0000_0100,
    body = 0b_0000_1000,
    hand = 0b_0001_0000,
    equipment = hand | body | head,//0b_0001_1100
    food = 0b_0010_0000,
    usualSlot = 0b_0011_1111,
    backpack = 0b_0100_0000,
    bigItem = 0b_1000_0000,
}