using System.Collections.Generic;

public class EntityShop : EntityData
{
    private string GetTip() => "LOMBARD ;-)";

    public EntityShop(float xpos, float zpos) : base(xpos, zpos)
    {
        Components.AddRange(new List<ComponentData>()
        {
            new ComponentModelPrefab(Dict.RectKeys.Shop),
            new ComponentInterractable(GetTip),
            new ComponentUICraftGroup(Dict.RecipeGroups.ShopDebug),
        });
    }    
}