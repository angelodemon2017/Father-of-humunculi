using UnityEngine;
using static OptimazeExtensions;

public class HomuWorkPlace : PrefabByComponentData
{
    [SerializeField] private int CountSlot;

    private ComponentHomuWorkPlace _component;

    public override int KeyType => TypeCache<HomuWorkPlace>.IdType;
    public override int KeyComponentData => TypeCache<ComponentHomuWorkPlace>.IdType;
    internal override ComponentData GetComponentData => new ComponentHomuWorkPlace(CountSlot);

    public override void Init(ComponentData componentData, EntityInProcess entityInProcess = null)
    {
        _component = componentData as ComponentHomuWorkPlace;
    }

    internal void SetHomu(int idSlot, long idEntity)
    {
        _component.SetSlot(idSlot, idEntity);
    }

    internal void ClearSlot(int idSlot)
    {
        _component.SetSlot(idSlot, -1);
    }
}