using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Entities/Base entity", order = 1)]
public class EntitySO : ScriptableObject
{
    public string Key;//??

    public List<ComponentSO> Components = new();

    internal virtual PropsData defaultData => new PropsData();

    public EntityData InitEntity()
    {
        return new EntityData(Key, defaultData);
    }
}