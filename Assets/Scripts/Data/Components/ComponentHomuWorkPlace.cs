using static OptimazeExtensions;

public class ComponentHomuWorkPlace : ComponentData
{
    public long[] slots;

    public ComponentHomuWorkPlace(int countSlots) : base(TypeCache<ComponentHomuWorkPlace>.IdType)
    {
        slots = new long[countSlots];
    }

    internal void SetSlot(int idSlot, long idEntity)
    {
        slots[idSlot] = idEntity;
    }
}