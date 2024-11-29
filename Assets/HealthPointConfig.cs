using UnityEngine;

public class HealthPointConfig : PrefabByComponentData
{
    [SerializeField] private int MaxHP;
    [SerializeField] private int RegenHP;

    public override string KeyComponentData => base.KeyComponentData;
    internal override ComponentData GetComponentData => new ComponentHPData()
    {
        CurrentHP = MaxHP,
        RegenHP = RegenHP,
    };

    public override void DoSecond(EntityData entity)
    {
        var chp = entity.Components.GetComponent<ComponentHPData>();
        if (chp != null)
        {
            if (Health(chp, chp.RegenHP))
            {
                entity.UpdateEntity();
            }
        }
    }

    private bool Health(ComponentHPData componentHP, int amount)
    {
        if (!componentHP.IsDeath && componentHP.CurrentHP < MaxHP)
        {
            componentHP.CurrentHP += amount;
            if (componentHP.CurrentHP > MaxHP)
            {
                componentHP.CurrentHP = MaxHP;
            }
            return true;
        }
        return false;
    }

    private void GetDamage()
    {

    }
}