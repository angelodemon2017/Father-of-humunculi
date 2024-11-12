using UnityEngine;

public class PlayerPresent : PrefabByComponentData
{
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
        if (message == Dict.Commands.DropItem)
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