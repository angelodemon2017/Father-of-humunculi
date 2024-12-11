using System;
using UnityEngine;

public class ComponentData
{
    internal long _idEntity;
    private string _cashName;
    public string KeyName
    {
        get
        {
            if (string.IsNullOrWhiteSpace(_cashName))
            {
                _cashName = GetType().Name;
            }
            return _cashName;
        }
    }
    public string AddingKey = string.Empty;
    public Action changed;

    internal void SetIdEntity(long id)
    {
        _idEntity = id;
    }

    public virtual void Init(Transform entityME) { }

    public virtual bool DoSecond()
    {
        return false;
    }
}