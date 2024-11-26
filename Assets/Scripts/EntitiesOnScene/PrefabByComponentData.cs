using UnityEngine;

public class PrefabByComponentData : MonoBehaviour
{
    public virtual string KeyComponent => string.Empty;
    public virtual string KeyComponentData => string.Empty;
    public virtual string GetDebugText => string.Empty;
    internal virtual bool _isNeedUpdate => false;
    internal virtual string AddingKey => string.Empty;
    internal virtual bool CanInterAct => false;

    internal virtual ComponentData GetComponentData => new ComponentData();

    internal virtual void PrepareEntityBeforeCreate(EntityData entityData)
    {

    }

    public virtual void Init(ComponentData componentData, EntityInProcess entityInProcess = null)
    {

    }

    internal virtual void UpdateComponent()
    {

    }

    internal virtual void VirtualDestroy()
    {

    }

    #region Config/Backend

    public virtual void ExecuteCommand(EntityData entity, string command, string message, WorldData worldData)
    {

    }

    public virtual void DoSecond(EntityData entity)
    {

    }

    #endregion
}