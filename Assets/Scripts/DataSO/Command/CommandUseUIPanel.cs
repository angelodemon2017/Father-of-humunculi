using UnityEngine;

[CreateAssetMenu(menuName = "Command/Command Use Entity With UI", order = 1)]
public class CommandUseUIPanel : CommandExecuterSO
{
    internal override string Key => typeof(ComponentInterractable).Name;

    public override void Execute(EntityData entity, string message, WorldData worldData)
    {
        var compPlayer = entity.Components.GetComponent<ComponentUICraftGroup>();

        if (compPlayer != null)
        {
            compPlayer.SetEntityOpener(long.Parse(message));
            entity.UpdateEntity();
        }
    }
}