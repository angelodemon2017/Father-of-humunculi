using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Components/Counter Component", order = 1)]
public class ComponentSOCounter : ComponentSO, ISeconder
{
    [SerializeField] private ParamConfig checkChance;
//    [SerializeField] int _chanceToAction;
    [SerializeField] private List<ParamConfig> paramsConfig = new();

    public void DoSecond(EntityData entity)
    {
        entity.Props.GetInt("");
        if (checkChance.GetI.GetChance())
        {
            //...
        }
    }

    public void DoSecond()
    {
/*        if (_chanceToAction.GetChance())
        {
            //??
        }/**/
    }
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