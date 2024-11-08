using UnityEngine;

[CreateAssetMenu(menuName = "Command/Command Drop Item By Player", order = 1)]
public class CommandDropItemByPlayer : CommandExecuterSO
{
    [SerializeField] private EntitySO _entityDroppedItem;

    internal override string Key => Dict.Commands.DropItem;

    public override void Execute(EntityData entity, string message, WorldData worldData)
    {
        var newEnt = _entityDroppedItem.CreateEntity(entity.Position.x, entity.Position.z);

        var compPlayer = entity.Components.GetComponent<ComponentPlayerId>();

        if (compPlayer != null)
        {
            var itemPresent = newEnt.Components.GetComponent<ComponentItemPresent>();
            itemPresent.SetItem(compPlayer.ItemHand);
            compPlayer.ItemHand.SetEmpty();

            worldData.AddEntity(newEnt);
        }
    }

    public static CommandData GetCommand(EntityData entity)
    {
        return new CommandData(entity.Id, Dict.Commands.DropItem);
    }
}