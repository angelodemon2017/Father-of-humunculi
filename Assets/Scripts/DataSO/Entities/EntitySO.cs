using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Entities/Base entity", order = 1)]
public class EntitySO : ScriptableObject
{
    public string Key;//??

    public List<ComponentSO> Components;

    public EntityData InitEntity()
    {
        var fields = new List<ParamConfig>();

        foreach (var c in Components)
        {
            //get some data template
            //            fields.Add(c);
        }

        return new EntityData()
        {
            //TODO fulling data fields
        };
    }
}