using UnityEngine;

public class PrefabByComponentData<T> : MonoBehaviour where T : ComponentData
{
    public virtual string KeyComponent => default(T).KeyName;

    public virtual void Init(T componentData)
    {

    }
}