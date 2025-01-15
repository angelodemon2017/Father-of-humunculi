using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EntityMonobehFabric
{
    private Dictionary<string, HashSet<EntityMonobeh>> _poolEntities = new();
    private EntitiesLibrary _entitiesLibrary;
    private Transform _parentTransform;

    public EntityMonobehFabric(EntitiesLibrary entitiesLibrary, Transform parentTransform)
    {
        _entitiesLibrary = entitiesLibrary;
        _parentTransform = parentTransform;
    }

    public void DestroyEntity(EntityMonobeh entityMonobeh)
    {
        var typeKey = entityMonobeh.EntityInProcess.EntityData.TypeKey;

        if (!_poolEntities.ContainsKey(typeKey))
        {
            _poolEntities.Add(typeKey, new());
        }

        _poolEntities[typeKey].Add(entityMonobeh);

        entityMonobeh.VirtualDestroy();
    }

    public EntityMonobeh Create(string key)
    {
        if (_poolEntities.ContainsKey(key) && _poolEntities[key].Count > 0)
        {
            var tempEMB = _poolEntities[key].First();
            _poolEntities[key].Remove(tempEMB);
            tempEMB.VirtualCreate();
            return tempEMB;
        }

        var entMon = _entitiesLibrary.GetConfig(key);
        return Object.Instantiate(entMon, _parentTransform);
    }
}