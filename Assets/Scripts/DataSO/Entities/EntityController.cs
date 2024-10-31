using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EntityController
{
    private static Dictionary<string, EntitySO> _entities = new();

    public static List<EntitySO> GetEntities()
    {
        Init();

        return _entities.Values.ToList();
    }

    public static EntitySO GetEntity(string key)
    {
        Init();

        return _entities[key];
    }

    private static void Init()
    {
        if (_entities.Count == 0)
        {
            var tempItems = Resources.LoadAll<EntitySO>(Config.PathEntityConfigs).ToList();

            tempItems.ForEach(i => _entities.Add(i.Key, i));
        }
    }
}