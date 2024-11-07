using UnityEngine;

[CreateAssetMenu(menuName = "Command/Command Use Counter", order = 1)]
public class CommandUseCounter : CommandExecuterSO
{
    [SerializeField] ItemConfig _itemConfig;

    internal override string Key => typeof(ComponentInterractable).Name;

    public override void Execute(EntityData entity, string message, WorldData worldData)
    {
        var counter = entity.Components.GetComponent<ComponentCounter>();

        if (counter._debugCounter > 0)
        {
            var idFromMessage = long.Parse(message);

            var touchedEntity = worldData.GetEntityById(idFromMessage);

            var inventoryComp = touchedEntity.Components.GetComponent<ComponentInventory>();

            var addedItem = new ItemData(_itemConfig);
            addedItem.Count = counter._debugCounter;
            counter._debugCounter = 0;

            inventoryComp.AddItem(addedItem);

            entity.UpdateEntity();
            touchedEntity.UpdateEntity();
        }
    }
}