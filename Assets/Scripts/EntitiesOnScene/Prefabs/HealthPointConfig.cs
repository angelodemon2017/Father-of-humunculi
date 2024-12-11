using System.Collections.Generic;
using UnityEngine;

public class HealthPointConfig : PrefabByComponentData
{
    [SerializeField] private EntityMonobeh _itemPrefab;
    [SerializeField] private List<ItemConfig> _dropItemsByDeath;
    [SerializeField] private int MaxHP;
    [SerializeField] private int RegenHP;

    private ComponentHPData _componentHP;

    public override string GetDebugText => $"HP:{_componentHP.CurrentHP}/{MaxHP}";
    public override string KeyComponentData => typeof(ComponentHPData).Name;
    internal override ComponentData GetComponentData => new ComponentHPData()
    {
        CurrentHP = MaxHP,
        RegenHP = RegenHP,
    };

    public override void Init(ComponentData componentData, EntityInProcess entityInProcess = null)
    {
        _componentHP = (ComponentHPData)componentData;
    }

    public override void DoSecond(EntityData entity)
    {
        var chp = entity.GetComponent<ComponentHPData>();
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

    public bool GetDamage(EntityData target, DamagerConfig damage)
    {
        var componentHP = target.GetComponent<ComponentHPData>();
        if (!componentHP.IsDeath && damage.GetDamage > 0)
        {
            componentHP.CurrentHP -= damage.GetDamage;
            if (componentHP.CurrentHP < 0)
            {
                componentHP.CurrentHP = 0;
                
                foreach (var drop in _dropItemsByDeath)
                {
                    DropItem(target, drop);
                }

                GameProcess.Instance.GameWorld.RemoveEntity(target.Id);
            }

            return true;
        }

        return false;
    }

    private void DropItem(EntityData target, ItemConfig item)
    {
        var randDecPos = new Vector2(SimpleExtensions.GetRandom(-2f, 2f), SimpleExtensions.GetRandom(-2f, 2f));
        var swiftPos = randDecPos.normalized * SimpleExtensions.GetRandom(1f, 2f);
        var newEnt = _itemPrefab.CreateEntity(target.Position.x + swiftPos.x, target.Position.z + swiftPos.y);
        var compItem = newEnt.GetComponent<ComponentItemPresent>();
        compItem.SetItem(new ItemData(item));

        GameProcess.Instance.GameWorld.AddEntity(newEnt);
    }
}