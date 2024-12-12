using System;

public class ComponentData
{
    internal long _idEntity;
    public readonly int KeyType;

    public int AddingKey = 0;
    public Action changed;

    protected ComponentData(int keyType)
    {
        KeyType = keyType;
    }

    internal void SetIdEntity(long id)
    {
        _idEntity = id;
    }

    public virtual bool DoSecond()
    {
        return false;
    }
}