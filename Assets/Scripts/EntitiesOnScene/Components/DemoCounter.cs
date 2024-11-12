using UnityEngine;

public class DemoCounter : PrefabByComponentData
{
    [SerializeField] private EntityMonobeh _demoMiniMob;
    [SerializeField] private ItemConfig _givingItem;
    [SerializeField] private ComponentCounter _defaultValues;

    public override string KeyComponent => typeof(DemoCounter).Name;
    public override string KeyComponentData => typeof(ComponentCounter).Name;

    public override string GetDebugText => $"res: {_component._debugCounter}";

    private ComponentCounter _component;

    internal override ComponentData GetComponentData => new ComponentCounter(_defaultValues);

    public override void Init(ComponentData componentData, EntityInProcess entityInProcess = null)
    {
        _component = (ComponentCounter)componentData;
    }

    public void PickEntity(EntityData entity, string message, WorldData worldData)
    {
        var counter = entity.Components.GetComponent<ComponentCounter>();

        if (counter._debugCounter > 0)
        {
            var idFromMessage = long.Parse(message);

            var touchedEntity = worldData.GetEntityById(idFromMessage);

            var inventoryComp = touchedEntity.Components.GetComponent<ComponentInventory>();

            var addedItem = new ItemData(_givingItem);
            addedItem.Count = counter._debugCounter;
            counter._debugCounter = 0;

            inventoryComp.AddItem(addedItem);

            entity.UpdateEntity();
            touchedEntity.UpdateEntity();

            worldData.AddEntity(_demoMiniMob.CreateEntity(entity.Position.x, entity.Position.z));
        }
    }
}