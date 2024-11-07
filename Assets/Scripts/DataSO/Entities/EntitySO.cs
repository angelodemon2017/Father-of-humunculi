using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Entities/Base entity", order = 1)]
public class EntitySO : ScriptableObject
{
    public string Key;//??

    public List<ComponentSO> Components = new();

    public List<CommandExecuterSO> Commands = new();

    public void InitOnScene(EntityMonobeh entityMonobeh)
    {
        Components.ForEach(c => c.InitOnScene(entityMonobeh));
    }

    public EntityData InitEntity(float xpos = 0, float zpos = 0)
    {
        var newEntity = new EntityData(xpos, zpos);

        newEntity.TypeKey = Key;
        foreach (var c in Components)
        {
            newEntity.Components.Add(c.GetComponentData);
        }

        return newEntity;
    }

    public void DoSecond(EntityData entityData)
    {
        foreach (var c in Components)
        {
            if (c is ISeconderEntity cs)
            {
                cs.DoSecond(entityData);
            }
        }
    }

    public void UseCommand(EntityData entity, string keyCommand, string message, WorldData worldData)
    {
        foreach (var c in Commands)
        {
            if (c.Key == keyCommand)
            {
                c.Execute(entity, message, worldData);
            }
        }
    }
}