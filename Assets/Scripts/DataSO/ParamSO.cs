using UnityEngine;

public class ParamSO : ScriptableObject
{//TODO Why need this class
    public string Key;
    public object Value;

    public virtual void Init(PropsData props)
    {

    }
}