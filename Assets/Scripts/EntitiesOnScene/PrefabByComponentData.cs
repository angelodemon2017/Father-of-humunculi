using UnityEngine;

public class PrefabByComponentData : MonoBehaviour
{
    public virtual string KeyComponent => string.Empty;
    public virtual string KeyComponentData => string.Empty;
    public virtual string GetDebugText => string.Empty;
    internal virtual bool _isNeedUpdate => false;

    internal virtual ComponentData GetComponentData { get; }

    public virtual void Init(ComponentData componentData, EntityInProcess entityInProcess = null)
    {

    }

    internal virtual void UpdateComponent()
    {

    }

    #region Config/Backend

    public virtual void ExecuteCommand(EntityData entity, string command, string message, WorldData worldData)
    {

    }

    #endregion
}