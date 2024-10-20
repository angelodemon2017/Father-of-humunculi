using System.Collections.Generic;

public class EntityMiniMob : EntityData
{
    public EntityMiniMob(float xpos, float zpos) : base(xpos, zpos)
    {
        Components.AddRange(new List<ComponentData>()
        {
            new ComponentModelPrefab("MiniMob"),
            new ComponentFSM("RandomWalking"),
    });
    }
}