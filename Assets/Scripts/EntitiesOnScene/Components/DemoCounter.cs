using UnityEngine;

public class DemoCounter : PrefabByComponentData
{
    [SerializeField] private ItemConfig _givingItem;
    [SerializeField] private ComponentCounter _defaultValues;

    private ComponentCounter _component;

    internal override bool CanInterAct => _component._debugCounter > 0;
    public ItemData GivingItem => new ItemData(_givingItem);
    public override string KeyComponentData => typeof(ComponentCounter).Name;
    public override string GetDebugText => $"res: {_component._debugCounter}";
    internal override ComponentData GetComponentData => new ComponentCounter(_defaultValues);

    public override void Init(ComponentData componentData, EntityInProcess entityInProcess = null)
    {
        _component = (ComponentCounter)componentData;
    }

    public void PickEntity(EntityData entity, string command, string message, WorldData worldData)
    {
        var counter = entity.Components.GetComponent<ComponentCounter>();

        if (counter._debugCounter > 0)
        {
            var idFromMessage = long.Parse(message);

            var touchedEntity = worldData.GetEntityById(idFromMessage);

            var addedItem = GivingItem;
            var invs = touchedEntity.Components.GetComponents(typeof(ComponentInventory).Name);
            foreach (ComponentInventory inv in invs)
            {
                if (inv.AvailableAddItem(addedItem))
                {
                    addedItem.Count = counter._debugCounter;

                    inv.AddItem(addedItem);
                    touchedEntity.UpdateEntity();

                    counter._debugCounter = 0;

                    break;
                }
            }

            entity.UpdateEntity();
            touchedEntity.UpdateEntity();
        }
    }

    public override void DoSecond(EntityData entity)
    {
        var cc = entity.Components.GetComponent<ComponentCounter>();
        if (cc != null)
        {
            if ((cc._maxCount == 0 || cc._debugCounter < cc._maxCount) &&
                cc._chanceUpper.GetChance())
            {
                cc._debugCounter++;
                entity.UpdateEntity();
            }
        }
    }
}