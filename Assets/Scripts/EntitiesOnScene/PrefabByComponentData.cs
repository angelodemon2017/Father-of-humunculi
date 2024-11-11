using UnityEngine;

public class PrefabByComponentData : MonoBehaviour
{
    public virtual string KeyComponent => string.Empty;
    public virtual string KeyComponentData => string.Empty;
    public virtual string GetDebugText => string.Empty;

    internal virtual ComponentData GetComponentData { get; }

    public virtual void Init(ComponentData componentData, EntityInProcess entityInProcess = null)
    {

    }

    public virtual void ExecuteCommand(EntityData entity, string message, WorldData worldData)
    {

    }
}