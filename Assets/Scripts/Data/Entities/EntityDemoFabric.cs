using System.Collections.Generic;

public class EntityDemoFabric : EntityData
{
    private string GetTip() => "Fabric";

    public EntityDemoFabric(float xpos, float zpos) : base(xpos, zpos)
    {
        Components.AddRange(new List<ComponentData>()
        {
            new ComponentModelPrefab(Dict.RectKeys.DemoFabric),
            new ComponentInterractable(GetTip),
            
//            new ComponentUICraftGroup(Dict.RecipeGroups.ShopDebug),
        });
    }

    public override void ApplyCommand(CommandData command)
    {
        if (command.KeyCommand == typeof(ComponentInterractable).Name)
        {
//            var com = Components.GetComponent<ComponentUICraftGroup>();
//            com.SetEntityOpener(long.Parse(command.Message));
        }

        base.ApplyCommand(command);
    }
}