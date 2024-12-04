using UnityEngine;

public class DamagerConfig : PrefabByComponentData
{
    [SerializeField] private int DefaultDamage;

    public int GetDamage => DefaultDamage;
}