using UnityEngine;

public class PrefabByComponentData : MonoBehaviour
{
    public virtual string KeyComponent => string.Empty;
    public virtual string KeyComponentData => string.Empty;
    public virtual string GetDebugText => string.Empty;

    public virtual void Init(ComponentData componentData, EntityInProcess entityInProcess = null)
    {

    }
}