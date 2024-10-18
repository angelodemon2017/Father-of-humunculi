using System.Collections.Generic;
using UnityEngine;

public class EntityPlayer : EntityData
{
    public override string DebugField => $"Your source:{Components.GetComponent<ComponentInventory>().SomeResource}";

    public EntityPlayer(float xpos, float zpos) : base(xpos, zpos)
    {
        Components.AddRange(new List<ComponentData>()
        {
            new ComponentModelPrefab("Player"),
            new ComponentFSM("Player"),//switch to net game
            new ComponentPlayerId(),
            new ComponentInventory(),
            new ComponentUIlabels(Color.white, 1.5f),
        });        
    }
}