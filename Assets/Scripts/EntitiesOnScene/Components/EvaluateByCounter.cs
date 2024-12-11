using System.Collections.Generic;
using UnityEngine;

public class EvaluateByCounter : PrefabByComponentData, IDepenceCounter
{
    [SerializeField] private int NeedToEvaluate;
    [SerializeField] private List<EntityMonobeh> NextEntity;

    public void CheckComponent(ComponentCounter counter, EntityData entityData)
    {
        if (counter._debugCounter >= NeedToEvaluate)
        {
            var pos = entityData.Position;
            GameProcess.Instance.GameWorld.RemoveEntity(entityData.Id);
            GameProcess.Instance.GameWorld.AddEntity(NextEntity.GetRandom().CreateEntity(pos.x, pos.z));
        }
    }
}