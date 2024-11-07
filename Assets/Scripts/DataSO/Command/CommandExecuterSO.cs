using UnityEngine;

public class CommandExecuterSO : ScriptableObject
{
    internal virtual string Key { get; }

    public virtual void Execute(EntityData entity, string message, WorldData worldData)
    {

    }
}