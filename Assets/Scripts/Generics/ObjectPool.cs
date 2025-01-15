using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectPool<T> where T : MonoBehaviour, IObjectOfPool
{
    private HashSet<T> _pool = new HashSet<T>();
    private T _prefab;
    private Transform _parentTransform;

    public ObjectPool(T prefab, Transform parentTransform)
    {
        _prefab = prefab;
        _parentTransform = parentTransform;
    }

    public void DestroyObject(T obj)
    {
        obj.VirtualDestroy();
        _pool.Add(obj);
    }

    public T Get()
    {
        if (_pool.Count > 0)
        {
            var pooledObject = _pool.First();
            _pool.Remove(pooledObject);
            pooledObject.VirtualCreate();
            return pooledObject;
        }

        return Object.Instantiate(_prefab, _parentTransform);
    }
}