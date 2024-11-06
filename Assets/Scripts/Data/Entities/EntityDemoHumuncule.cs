using System.Collections.Generic;
using UnityEngine;

public class EntityDemoHumuncule : EntityData
{
    public override string DebugField => $"HUMUNCULE <_>";

    public EntityDemoHumuncule(float xpos, float zpos) : base(xpos, zpos)
    {
        Components.AddRange(new List<ComponentData>()
        {
            new ComponentModelPrefab("DemoHumuncule"),
//            new ComponentFSM("RandomWalking"),
            new ComponentUIlabels(Color.blue),
    });
    }
}