using UnityEngine;
using static OptimazeExtensions;

public class DamagerConfig : PrefabByComponentData
{//is it necessary?
    public override int KeyType => TypeCache<DamagerConfig>.IdType;
    [SerializeField] private int DefaultDamage;

    public int GetDamage => DefaultDamage;
}