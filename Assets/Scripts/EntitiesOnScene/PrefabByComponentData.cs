using UnityEngine;
using static OptimazeExtensions;

public class PrefabByComponentData : MonoBehaviour
{
    public virtual int KeyType => TypeCache<PrefabByComponentData>.IdType;
    public virtual int KeyComponentData => TypeCache<ComponentDummy>.IdType;
    public virtual string GetDebugText => string.Empty;
    internal virtual bool _isNeedUpdate => false;
    internal virtual int AddingKey => 0;
    internal virtual bool CanInterAct => false;

    internal virtual ComponentData GetComponentData => new ComponentDummy();

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

    internal virtual void PrepareEntityBeforeCreate(EntityData entityData)
    {

    }

    public virtual void ExecuteCommand(EntityData entity, string command, string message, WorldData worldData)
    {

    }

    public virtual void DoSecond(EntityData entity)
    {

    }

    #endregion
}