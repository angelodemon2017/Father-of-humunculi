using UnityEngine;

public class PlayerPresent : PrefabByComponentData
{
    private const char splitter = '^';

    [SerializeField] private EntityMonobeh _dropItem;
    [SerializeField] private ItemConfig _emptyItem;
    [SerializeField] private EntityMonobeh _entityMonobeh;

    private ComponentPlayerId _component;

    public override string KeyComponent => typeof(PlayerPresent).Name;
    public override string KeyComponentData => typeof(ComponentPlayerId).Name;

    internal override ComponentData GetComponentData => new ComponentPlayerId(new ItemData(_emptyItem));

    public override void Init(ComponentData componentData, EntityInProcess entityInProcess = null)
    {
        _component = (ComponentPlayerId)componentData;

        CameraController.Instance.SetTarget(_entityMonobeh.transform);
        UIPlayerManager.Instance.InitEntity(_entityMonobeh);
    }

    public override void ExecuteCommand(EntityData entity, string command, string message, WorldData worldData)
    {
        switch (command)
        {
            case Dict.Commands.SlotDrag:
                var playerComp = entity.Components.GetComponent<ComponentPlayerId>();

                var args = message.Split(splitter);
                var invEnt = worldData.GetEntityById(long.Parse(args[0]));
                var invComp = invEnt.Components.GetComponent<ComponentInventory>(args[1]);
                var slotInv = invComp.Items[int.Parse(args[2])];

                playerComp.ItemHand.Replace(slotInv);
                slotInv.SetEmpty();

                entity.UpdateEntity();
                invEnt.UpdateEntity();
                break;
            case Dict.Commands.DropItem:
                DropItem(entity, worldData);
                break;
            default:
                break;
        }
    }

    private void DropItem(EntityData entity, WorldData worldData)
    {
        var compPlayer = entity.Components.GetComponent<ComponentPlayerId>();

        if (compPlayer != null)
        {
            var newEnt = _dropItem.CreateEntity(entity.Position.x, entity.Position.z);
            var itemPresent = newEnt.Components.GetComponent<ComponentItemPresent>();
            itemPresent.SetItem(compPlayer.ItemHand);
            compPlayer.ItemHand.SetEmpty();

            worldData.AddEntity(newEnt);
        }
    }

    public static CommandData GetCommandDragItem(long idEntity, string addingKey, int idSlot)
    {
        return new CommandData()
        {
            KeyComponent = typeof(PlayerPresent).Name,
            KeyCommand = Dict.Commands.SlotDrag,
            Message = $"{idEntity}{splitter}{addingKey}{splitter}{idSlot}",
        };
    }

    public CommandData GetCommandDropItem(EntityData entityData)
    {
        return new CommandData()
        {
            KeyComponent = KeyComponent,
            Message = Dict.Commands.DropItem,
        };
    }
}