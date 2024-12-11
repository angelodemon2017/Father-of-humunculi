using UnityEngine;

[RequireComponent(typeof(DemoCounter))]
public class SpawnerByCounter : PrefabByComponentData, IDepenceCounter
{
    [SerializeField] private DemoCounter _demoCounter;
    [SerializeField] private int _needCounterForSpawn;
    [SerializeField] private int _maxEntities;
    [SerializeField] private float _spawnDistanceMin;
    [SerializeField] private float _spawnDistanceMax;
    [SerializeField] private float _distanceDetach;
    [SerializeField] private EntityMonobeh _entityForSpawn;
    
    public override string KeyComponentData => typeof(ComponentSpawner).Name;

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
//        CheckAndSpawn(entity);
    }

    private void DetachEnt(EntityData entity)
    {
        long forDel = -1;
        var compCS = entity.Components.GetComponent<ComponentSpawner>();
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

    private bool CheckAndSpawn(EntityData entity)
    {
        var compCount = entity.Components.GetComponent<ComponentCounter>();
        var compCS = entity.Components.GetComponent<ComponentSpawner>();

        if (compCount._debugCounter >= _needCounterForSpawn && compCS.Entities.Count < _maxEntities)
        {
            var randDecPos = new Vector2(SimpleExtensions.GetRandom(-2f, 2f), SimpleExtensions.GetRandom(-2f, 2f));
            var swiftPos = randDecPos.normalized * SimpleExtensions.GetRandom(_spawnDistanceMin, _spawnDistanceMax);

            var newEnt = _entityForSpawn.CreateEntity(entity.Position.x + swiftPos.x, entity.Position.z + swiftPos.y);
            compCount._debugCounter -= _needCounterForSpawn;

            if (newEnt.TypeKey == "ItemPresent")
            {
                var compItem = newEnt.Components.GetComponent<ComponentItemPresent>();
                if (compItem != null)
                {
                    compItem.SetItem(_demoCounter.GivingItem);
                }
            }
            else
            {
                var compFSM = newEnt.Components.GetComponent<ComponentFSM>();
                if (compFSM != null)
                {
                    compFSM.EntityOfBirth = entity.Id;
                    compFSM.EntityTarget = entity.Id;
                }
            }

            var newId = GameProcess.Instance.GameWorld.AddEntity(newEnt);
            compCS.Entities.Add(newId);

            return true;
        }

        return false;
    }

    public void CheckComponent(ComponentCounter counter, EntityData entityData)
    {
        var compCS = entityData.Components.GetComponent<ComponentSpawner>();

        if (counter._debugCounter >= _needCounterForSpawn && compCS.Entities.Count < _maxEntities)
        {
            var randDecPos = new Vector2(SimpleExtensions.GetRandom(-2f, 2f), SimpleExtensions.GetRandom(-2f, 2f));
            var swiftPos = randDecPos.normalized * SimpleExtensions.GetRandom(_spawnDistanceMin, _spawnDistanceMax);

            var newEnt = _entityForSpawn.CreateEntity(entityData.Position.x + swiftPos.x, entityData.Position.z + swiftPos.y);
            counter._debugCounter -= _needCounterForSpawn;

            if (newEnt.TypeKey == "ItemPresent")
            {
                var compItem = newEnt.Components.GetComponent<ComponentItemPresent>();
                if (compItem != null)
                {
                    compItem.SetItem(_demoCounter.GivingItem);
                }
            }
            else
            {
                var compFSM = newEnt.Components.GetComponent<ComponentFSM>();
                if (compFSM != null)
                {
                    compFSM.EntityOfBirth = entityData.Id;
                    compFSM.EntityTarget = entityData.Id;
                }
            }

            var newId = GameProcess.Instance.GameWorld.AddEntity(newEnt);
            compCS.Entities.Add(newId);
        }
    }
}