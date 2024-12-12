using UnityEngine;
using static OptimazeExtensions;

public class PickerItemsByCounter : PrefabByComponentData
{
    public override int KeyType => TypeCache<PickerItemsByCounter>.IdType;
    [SerializeField] private int CounterToItemCount;
    [SerializeField] private ItemConfig _givingItem;//may be list of item
    [SerializeField] private DemoCounter _demoCounter;
    [SerializeField] private CanvasUITempMessage _canvasUITempMessage;

    public ItemData GivingItem => new ItemData(_givingItem);

    public void PickEntity(EntityData entity, string command, string message, WorldData worldData)
    {
        var counter = entity.GetComponent<ComponentCounter>(_demoCounter.AddingKey);

        if (counter._debugCounter > CounterToItemCount)
        {
            var idFromMessage = long.Parse(message);

            var touchedEntity = worldData.GetEntityById(idFromMessage);

            var addedItem = GivingItem;
            var invs = touchedEntity.GetComponents<ComponentInventory>();
            foreach (ComponentInventory inv in invs)
            {
                if (inv.AvailableAddItem(addedItem))
                {
                    addedItem.Count = counter._debugCounter / CounterToItemCount;

                    inv.AddItem(addedItem);
                    touchedEntity.UpdateEntity();

                    counter._debugCounter %= CounterToItemCount;

                    break;
                }
            }

            entity.UpdateEntity();
            touchedEntity.UpdateEntity();
        }
        else
        {
            _canvasUITempMessage.ShowDefMessage(entity);
        }
    }
}