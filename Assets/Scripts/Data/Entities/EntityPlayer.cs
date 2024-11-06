using System.Collections.Generic;
using UnityEngine;

public class EntityPlayer : EntityData
{
    public override string DebugField => $"score:{ScoreDebug}";

    public int ScoreDebug;

    public EntityPlayer(float xpos, float zpos) : base(xpos, zpos)
    {
        Components.AddRange(new List<ComponentData>()
        {
            new ComponentModelPrefab("Player"),
//            new ComponentFSM("Player"),//switch to net game
//            new ComponentPlayerId(),
            new ComponentInventory(),
            new ComponentUIlabels(Color.white, 1.5f),
        });
    }

    public override void ApplyCommand(CommandData command)
    {
        base.ApplyCommand(command);
    }
}