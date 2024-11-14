using UnityEngine;

[RequireComponent(typeof(DemoCounter))]
public class SpawnerByCounter : PrefabByComponentData
{
    [SerializeField] private int _needCounterForSpawn;
    [SerializeField] private int _maxEntities;
    [SerializeField] private EntityMonobeh _entityForSpawn;

    public override string KeyComponent => typeof(SpawnerByCounter).Name;
    public override string KeyComponentData => typeof(ComponentSpawner).Name;

    internal override ComponentData GetComponentData => new ComponentSpawner() 
    {
        NeedCounterForSpawn = _needCounterForSpawn,
        KeyEntity = _entityForSpawn.name,
        MaxEntityNear = _maxEntities,
    };
}