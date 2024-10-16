using System;
using UnityEngine;

public abstract class ComponentData
{
    public string KeyName => GetType().Name;
    public Action changed;

    public virtual void Init(Transform entityME) { }

    public virtual void DoSecond()
    {

    }
}

public class ComponentPlayerId : ComponentData
{
    public string PlayerName;
}