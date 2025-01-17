using System.Collections.Generic;
using UnityEngine;
using static OptimazeExtensions;

public class DemoCounter : PrefabByComponentData
{
    public override int KeyType => TypeCache<DemoCounter>.IdType;
    [SerializeField] private ComponentCounter _defaultValues;
    [SerializeField] private List<PrefabByComponentData> _chekers;
    [SerializeField] private int _secondForTotal;

    [SerializeField] private string _debugCounter;

    private ComponentCounter _component;

    internal override bool _isNeedUpdate => true;
    internal override int AddingKey => _defaultValues.AddingKey;
    internal override bool CanInterAct => _component._debugCounter > 0;//TODO Big quest
    public override int KeyComponentData => TypeCache<ComponentCounter>.IdType;
    public override string GetDebugText => $"res: {(_component == null ? 0 : _component._debugCounter)}";
    internal override ComponentData GetComponentData => new ComponentCounter(_defaultValues);

    private void OnValidate()
    {
        _secondForTotal = _defaultValues._chanceUpper <= 0 ? -1 : (_defaultValues._maxCount - _defaultValues._debugCounter) * 100 / _defaultValues._chanceUpper;
    }

    public override void Init(ComponentData componentData, EntityInProcess entityInProcess = null)
    {
        _component = (ComponentCounter)componentData;
    }

    internal override void UpdateComponent()
    {
        _debugCounter = $"{_component._debugCounter}";
    }

    public override void DoSecond(EntityData entity)
    {
        var cc = entity.GetComponent<ComponentCounter>(AddingKey);
        if (cc != null)
        {
            if ((cc._maxCount == 0 || cc._debugCounter < cc._maxCount) &&
                cc._chanceUpper.GetChance())
            {
                cc._debugCounter++;
                entity.UpdateEntity();
            }
            if (cc._debugCounter > 0)
            {
                foreach (var chek in _chekers)
                {
                    if (chek is IDepenceCounter depenceCounter)
                    {
                        depenceCounter.CheckComponent(cc, entity);
                    }
                }
            }
        }
    }
}