using System;
using System.Collections.Generic;

[Serializable]
public class ComponentSpawner : ComponentData
{
    public int NeedCounterForSpawn;
    public int MaxEntityNear;
    public string KeyEntity;
    public List<long> Entities = new();

    internal override void UpdateAfterEntityUpdate(EntityData entity)
    {
        var compCount = entity.Components.GetComponent<ComponentCounter>();
        
        if (compCount._debugCounter >= NeedCounterForSpawn && Entities.Count < MaxEntityNear)
        {
            var newEnt = EntitiesLibrary.Instance.GetConfig(KeyEntity).CreateEntity(entity.Position.x, entity.Position.z);
            compCount._debugCounter -= NeedCounterForSpawn;

            var compFSM = newEnt.Components.GetComponent<ComponentFSM>();
            if (compFSM != null)
            {
                compFSM.EntityOfBirth = entity.Id;
                compFSM.EntityTarget = entity.Id;
            }

            var newId = GameProcess.Instance.GameWorld.AddEntity(newEnt);
            Entities.Add(newId);
        }        
    }
}