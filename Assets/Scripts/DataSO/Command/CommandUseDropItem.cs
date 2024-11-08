using UnityEngine;

[CreateAssetMenu(menuName = "Command/Command Use Dropped item", order = 1)]
public class CommandUseDropItem : CommandExecuterSO
{
    internal override string Key => typeof(ComponentInterractable).Name;

    public override void Execute(EntityData entity, string message, WorldData worldData)
    {
        var comItPr = entity.Components.GetComponent<ComponentItemPresent>();

        if (comItPr != null)
        {
            var idEnt = long.Parse(message);

            var entUsed = worldData.GetEntityById(idEnt);

            var invComp = entUsed.Components.GetComponent<ComponentInventory>();

            if (invComp != null)
            {
                invComp.AddItem(comItPr.ItemData);
                entUsed.UpdateEntity();

                worldData.RemoveEntity(entity.Id);
            }
        }
    }
}