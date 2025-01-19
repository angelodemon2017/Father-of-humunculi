using UnityEngine;
using static OptimazeExtensions;

public class HungerPresent : PrefabByComponentData
{
    public override int KeyType => TypeCache<HungerPresent>.IdType;

    [SerializeField] private int _hungerBySecond;
    [SerializeField] private int _maxStarvation;
    [SerializeField] private HealthPointConfig _healthPointConfig;
    [SerializeField] private Damage _damageByHunger;

    public override int KeyComponentData => TypeCache<ComponentHunger>.IdType;
    internal override ComponentData GetComponentData => new ComponentHunger(_maxStarvation);

    public override void DoSecond(EntityData entity)
    {
        var cmp = entity.GetComponent<ComponentHunger>();

        if (cmp.ApplyHunger(_hungerBySecond))
        {
            var hpc = entity.GetConfig.GetMyComponent<HealthPointConfig>();
            hpc.GetDamage(entity, _damageByHunger);
        }
        cmp.CalcGorgingBySecond();

        entity.UpdateEntity();
    }
}