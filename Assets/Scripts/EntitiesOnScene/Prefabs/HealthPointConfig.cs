using System.Collections.Generic;
using UnityEngine;
using static OptimazeExtensions;

public class HealthPointConfig : PrefabByComponentData
{
    public override int KeyType => TypeCache<HealthPointConfig>.IdType;
    [SerializeField] private EntityMonobeh _itemPrefab;
    [SerializeField] private List<ItemConfig> _dropItemsByDeath;
    [SerializeField] private int InitMaxHP;
    [SerializeField] private int RegenHP;
    [SerializeField] private int TimeoutRegenAfterDamage;

    private ComponentHPData _componentHP;

    public override string GetDebugText => $"HP:{_componentHP.CurrentHP}/{InitMaxHP}";
    public override int KeyComponentData => TypeCache<ComponentHPData>.IdType;
    internal override ComponentData GetComponentData => new ComponentHPData(InitMaxHP, RegenHP);

    public override void Init(ComponentData componentData, EntityInProcess entityInProcess = null)
    {
        _componentHP = (ComponentHPData)componentData;
    }

    public override void DoSecond(EntityData entity)
    {
        var chp = entity.GetComponent<ComponentHPData>();
        if (chp != null)
        {
            if (chp.TimeoutRegen > 0)
            {
                chp.TimeoutRegen--;
            }
            else
            {
                if (Health(chp, chp.RegenHP))
                {
                    entity.UpdateEntity();
                }
            }
        }
    }

    private bool Health(ComponentHPData componentHP, int amount)
    {
        if (!componentHP.IsDeath && componentHP.CurrentHP < InitMaxHP)
        {
            componentHP.CurrentHP += amount;
            if (componentHP.CurrentHP > InitMaxHP)
            {
                componentHP.CurrentHP = InitMaxHP;
            }
            return true;
        }
        return false;
    }

    public bool GetDamage(EntityData target, Damage damage)
    {
        var componentHP = target.GetComponent<ComponentHPData>();
        if (!componentHP.IsDeath && damage.GetDamage > 0)
        {
            componentHP.CurrentHP -= damage.GetDamage;
            componentHP.TimeoutRegen = TimeoutRegenAfterDamage;
            if (componentHP.CurrentHP <= 0)
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