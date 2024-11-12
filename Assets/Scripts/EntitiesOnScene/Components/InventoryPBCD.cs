using UnityEngine;

public class InventoryPBCD : PrefabByComponentData
{
    [SerializeField] private EntityMonobeh _dropItem;
    [SerializeField] private int _maxItems = 5;
    private ComponentInventory _component;

    public override string KeyComponent => typeof(InventoryPBCD).Name;
    public override string KeyComponentData => typeof(ComponentInventory).Name;

    internal override ComponentData GetComponentData => new ComponentInventory(_maxItems);

    public override void Init(ComponentData componentData, EntityInProcess entityInProcess = null)
    {
        _component = (ComponentInventory)componentData;
    }

    public override void ExecuteCommand(EntityData entity, string message, WorldData worldData)
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