using System.Collections.Generic;

public class EntityFlower : EntityData
{
    public EntityFlower(float xpos, float zpos) : base(xpos, zpos)
    {
        Components.AddRange(new List<ComponentData>()
        {
            new ComponentModelPrefab("MiniMob"),
        });
    }
}