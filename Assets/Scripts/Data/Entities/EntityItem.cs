using System.Collections.Generic;
using System.Linq;

public class EntityItem : EntityData
{
    public EntityItem(EnumItem idRes, float xpos, float zpos) : base(xpos, zpos)
    {
        Components.AddRange(new List<ComponentData>()
        {
            new ComponentItemPresent(idRes),
            new ComponentInterractable(GetTip),
        });
    }

    public EntityItem(ItemData item, float xpos, float zpos) : base(xpos, zpos)
    {
        Components.AddRange(new List<ComponentData>()
        {
            new ComponentItemPresent(item),
            new ComponentInterractable(GetTip),
        });
    }

    private string GetTip()
    {
        var component = Components.GetComponent<ComponentItemPresent>();
        return $"*{component.Tip}*";
    }

    public override void ApplyCommand(CommandData command)
    {
        if (command.KeyCommand == typeof(ComponentInterractable).Name)
        {
            var component = Components.GetComponent<ComponentItemPresent>();            ;

            var ent = worldData.entityDatas.FirstOrDefault(e => $"{e.Id}" == command.Message);
            var inv = ent.Components.GetComponent<ComponentInventory>();
            inv.AddItem(component.ItemData);
            ent.UpdateEntity();

            //PICK UP
            worldData.RemoveEntity(Id);
        }

        base.ApplyCommand(command);
    }
}