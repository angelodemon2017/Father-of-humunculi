﻿using System;
using UnityEngine;

public class ComponentData
{
    internal long _idEntity;
    public string KeyName => GetType().Name;
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

    internal virtual void UpdateAfterEntityUpdate(EntityData entity)
    {

    }
}