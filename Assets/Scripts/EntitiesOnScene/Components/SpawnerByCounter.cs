using UnityEngine;
using static OptimazeExtensions;
using static Helper;

[RequireComponent(typeof(DemoCounter))]
public class SpawnerByCounter : PrefabByComponentData, IDepenceCounter
{
    public override int KeyType => TypeCache<SpawnerByCounter>.IdType;
    [SerializeField] private ItemConfig _givingItem;
    [SerializeField] private DemoCounter _demoCounter;
    [SerializeField] private int _needCounterForSpawn;
    [SerializeField] private int _maxEntities;
    [SerializeField] private float _spawnDistanceMin;
    [SerializeField] private float _spawnDistanceMax;
    [SerializeField] private float _distanceDetach;
    [SerializeField] private EntityMonobeh _entityForSpawn;

    public ItemData GivingItem => new ItemData(_givingItem);
    public override int KeyComponentData => TypeCache<ComponentSpawner>.IdType;

    internal override ComponentData GetComponentData => new ComponentSpawner() 
    {
        NeedCounterForSpawn = _needCounterForSpawn,
        KeyEntity = _entityForSpawn.GetTypeKey,
        DistanceSpawnMin = _spawnDistanceMin,
        DistanceSpawnMax = _spawnDistanceMax,
        MaxEntityNear = _maxEntities,
        DistanceDetach = _distanceDetach,
    };

    public override void DoSecond(EntityData entity)
    {
        DetachEnt(entity);
    }

    private void DetachEnt(EntityData entity)
    {
        long forDel = -1;
        var compCS = entity.GetComponent<ComponentSpawner>();
        foreach (var entId in compCS.Entities)
        {
            if (GameProcess.Instance.GameWorld.HaveEnt(entId))
            {
                var tempEnt = GameProcess.Instance.GameWorld.GetEntityById(entId);
                if (Vector3.Distance(entity.Position, tempEnt.Position) > compCS.DistanceDetach)
                {
                    forDel = entId;
                    break;
                }
            }
            else
            {
                forDel = entId;
                break;
            }
        }
        if (forDel != -1)
        {
            compCS.Entities.Remove(forDel);
        }
    }

    public void CheckComponent(ComponentCounter counter, EntityData entityData)
    {
        var compCS = entityData.GetComponent<ComponentSpawner>();

        if (counter._debugCounter >= _needCounterForSpawn && compCS.Entities.Count < _maxEntities)
        {
            var swiftPos = GetRandomPointOfCircle(_spawnDistanceMin, _spawnDistanceMax);

            var newEnt = _entityForSpawn.CreateEntity(entityData.Position.x + swiftPos.x, entityData.Position.z + swiftPos.y);
            if (newEnt.TypeKey == Dict.SpecComponents.ItemPresent)
            {
                var compItem = newEnt.GetComponent<ComponentItemPresent>();
                if (compItem != null)
                {
                    compItem.SetItem(GivingItem);
                }
            }
            else
            {
                var compFSM = newEnt.GetComponent<ComponentFSM>();
                if (compFSM != null)
                {
                    compFSM.EntityOfBirth = entityData.Id;
                    compFSM.EntityTarget = entityData.Id;
                }
            }

            var newId = GameProcess.Instance.GameWorld.AddEntity(newEnt, true);
            if (newId != -1)
            {
                counter._debugCounter -= _needCounterForSpawn;
                compCS.Entities.Add(newId);
            }
        }
    }
}