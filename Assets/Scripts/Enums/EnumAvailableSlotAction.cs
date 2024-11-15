/// <summary>
/// Need to slots
/// </summary>
[System.Flags]
public enum EnumAvailableSlotAction : byte
{
    none = 0b_0000,
    drag = 0b_0000_0001,
    drop = 0b_0000_0010,
    clickLMB = 0b_0000_0100,
    clickRMB = 0b_0000_1000,
    clickMMB = 0b_0001_0000,
}