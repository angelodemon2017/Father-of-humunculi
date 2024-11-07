using UnityEngine;

[CreateAssetMenu(menuName = "Command/Command Spawn Entity", order = 1)]
public class CommandSpawnEntity : CommandExecuterSO
{
    [SerializeField] EntitySO _entity;

    internal override string Key => typeof(ComponentInterractable).Name;//???

    public override void Execute(EntityData entity, string message, WorldData worldData)
    {
        worldData.AddEntity(_entity.CreateEntity(entity.Position.x, entity.Position.z));
    }
}