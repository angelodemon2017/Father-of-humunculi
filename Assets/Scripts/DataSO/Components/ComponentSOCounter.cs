using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Components/Counter Component", order = 1)]
public class ComponentSOCounter : ComponentSO, ISeconder
{
    public ItemActionConfig WhenActionChance;

    public void DoSecond(EntityData entity)
    {
        if (entity.Props is ICounterData data)
        {
            if (data.Chance.GetChance())
            {
                WhenActionChance.ApplyItem(null, entity);
            }
        }
    }

    public void DoSecond()
    {

    }
}

[System.Serializable]
public class PropsDataCounter : PropsData, ICounterData
{
    public int Chance => 0;
    public int Count => 0;
}

interface ICounterData
{
    int Chance { get; }
    int Count { get; }
}

[System.Serializable]
public class ParamConfig
{
    public string Key;
    public TypeParam typeParam;
    public string Value;

    public int GetI => int.Parse(Value);

    public float GetF => float.Parse(Value);
}

public enum TypeParam
{
    _str,
    _int,
    _float,
}