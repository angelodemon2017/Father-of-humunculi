using System;
using System.Collections.Generic;

[Serializable]
public class ComponentSpawner : ComponentData
{
    public int NeedCounterForSpawn;
    public int MaxEntityNear;
    public float DistanceSpawnMin;
    public float DistanceSpawnMax;
    public string KeyEntity;
    public List<long> Entities = new();

/*    public override bool DoSecond()
    {
        var resultDetach = DetachEnt();

        var resultSpawn = CheckAndSpawn();

        return resultDetach || resultSpawn;
    }

    private bool DetachEnt()
    {
        long forDel = -1;
        var thatEnt = GameProcess.Instance.GameWorld.GetEntityById(_idEntity);
        foreach (var entId in Entities)
        {
            var tempEnt = GameProcess.Instance.GameWorld.GetEntityById(entId);
            if (tempEnt == null)
            {
                forDel = entId;
                break;
            }
            else
            {
                if (Vector3.Distance(thatEnt.Position, tempEnt.Position) > 4f)
                {
                    forDel = entId;
                    break;
                }
            }
        }
        if (forDel != -1)
        {
            Entities.Remove(forDel);
            return true;
        }

        return false;
    }

    private bool CheckAndSpawn()
    {
        var entity = GameProcess.Instance.GameWorld.GetEntityById(_idEntity);

        var compCount = entity.Components.GetComponent<ComponentCounter>();

        if (compCount._debugCounter >= NeedCounterForSpawn && Entities.Count < MaxEntityNear)
        {
            var randDecPos = new Vector2(UnityEngine.Random.Range(-2f, 2f), UnityEngine.Random.Range(-2f, 2f));
            var swiftPos = randDecPos.normalized * UnityEngine.Random.Range(DistanceSpawnMin, DistanceSpawnMax);

            var newEnt = EntitiesLibrary.Instance.GetConfig(KeyEntity).CreateEntity(entity.Position.x + swiftPos.x, entity.Position.z + swiftPos.y);
            compCount._debugCounter -= NeedCounterForSpawn;

            var compFSM = newEnt.Components.GetComponent<ComponentFSM>();
            if (compFSM != null)
            {
                compFSM.EntityOfBirth = entity.Id;
                compFSM.EntityTarget = entity.Id;
            }
            var compItem = newEnt.Components.GetComponent<ComponentItemPresent>();
            if (compItem != null)
            {
                var entConfig = EntitiesLibrary.Instance.GetConfig(entity.TypeKey);
                var dcComp = entConfig.PrefabsByComponents.GetComponent<DemoCounter>();
                compItem.SetItem(dcComp.GivingItem);
            }

            var newId = GameProcess.Instance.GameWorld.AddEntity(newEnt);
            Entities.Add(newId);

            return true;
        }

        return false;
    }/**/
}