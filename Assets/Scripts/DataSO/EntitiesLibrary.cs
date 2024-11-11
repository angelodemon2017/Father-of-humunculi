using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Entity Library", order = 1)]
public class EntitiesLibrary : ScriptableObject
{
    [SerializeField] private List<EntityMonobeh> _entities;
    private Dictionary<string, EntityMonobeh> _cashEntities = new();

    public EntityMonobeh GetConfig(string key)
    {
        Init();

        return _cashEntities[key];
    }

    private void Init()
    {
        if (_cashEntities.Count == 0)
        {
            foreach (var ent in _entities)
            {
                _cashEntities.Add(ent.gameObject.name, ent);
            }
        }
    }
}