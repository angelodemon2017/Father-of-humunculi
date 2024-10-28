using System.Collections.Generic;
using System.Linq;

public class EntityGoldBush : EntityData
{
    private string GetTip() => "Need Gold";

    public EntityGoldBush(float xpos, float zpos) : base(xpos, zpos)
    {
        Components.AddRange(new List<ComponentData>()
        {
            new ComponentModelPrefab(Dict.RectKeys.GoldBush),
            new ComponentInterractable(GetTip),
        });
    }

    public override void ApplyCommand(CommandData command)
    {
        if (command.Component == typeof(ComponentInterractable).Name)
        {
            var ent = worldData.entityDatas.FirstOrDefault(e => $"{e.Id}" == command.Message);
            var inv = ent.Components.GetComponent<ComponentInventory>();

            if (inv.SubtrackItems(EnumItem.Gold, 1))
            {
                inv.AddItem(new ItemData(ItemsController.GetItem(EnumItem.Tablet)));
            }
        }

        base.ApplyCommand(command);
    }
}