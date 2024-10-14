using UnityEngine;

public class PrefabByComponentData : MonoBehaviour
{
    public virtual string KeyComponent => string.Empty;

    public virtual void Init(ComponentData componentData)
    {

    }
}