using System.Collections.Generic;

public class EntityShop : EntityData
{
    public EntityShop(float xpos, float zpos) : base(xpos, zpos)
    {
        Components.AddRange(new List<ComponentData>()
        {
            new ComponentModelPrefab(Dict.RectKeys.Shop),
        });
    }
}