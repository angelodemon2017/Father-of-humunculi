using UnityEngine;
using static OptimazeExtensions;

public class HungerPresent : PrefabByComponentData
{
    public override int KeyType => TypeCache<HungerPresent>.IdType;

    [SerializeField] private int _hungerBySecond;

    internal override ComponentData GetComponentData => new ComponentHunger();

    public override void DoSecond(EntityData entity)
    {
        var cmp = entity.GetComponent<ComponentHunger>();

        if (cmp.ApplyHunger(_hungerBySecond))
        {
            //TODO take damage
        }
    }
}