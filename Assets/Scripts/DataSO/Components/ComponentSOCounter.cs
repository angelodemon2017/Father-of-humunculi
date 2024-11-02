using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Components/Counter Component", order = 1)]
public class ComponentSOCounter : ComponentSO, ISeconder
{
    [SerializeField] int _chanceToAction;
    [SerializeField] private List<ParamConfig> paramsConfig = new();

    public void DoSecond(EntityData entity)
    {
        entity.Props.GetInt("");
    }

    public void DoSecond()
    {
        if (_chanceToAction.GetChance())
        {
            //??
        }
    }
}

[System.Serializable]
public class ParamConfig
{
    public string Key;
    public TypeParam typeParam;
    public string Value;
}

public enum TypeParam
{
    _str,
    _int,
    _float,
}