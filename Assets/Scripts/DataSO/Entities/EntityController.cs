using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EntityController
{
    private static List<EntitySO> _entities = new();

    public static List<EntitySO> GetEntities()
    {
        Init();

        return _entities;
    }

    public static EntitySO GetEntity()
    {
        Init();

        return _entities.FirstOrDefault();
    }

    private static void Init()
    {
        if (_entities.Count == 0)
        {
            var tempItems = Resources.LoadAll<EntitySO>(Config.PathEntityConfigs).ToList();
            tempItems.ForEach(i => _entities.Add(i));
        }
    }
}