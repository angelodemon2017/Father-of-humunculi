using System.Collections.Generic;
using UnityEngine;
using static OptimazeExtensions;

public class EntityMonobeh : MonoBehaviour
{
    protected int Uid;
    [SerializeField] private string TypeKey;
    [SerializeField] private float RadEntity;
    [SerializeField] private List<PrefabByComponentData> _prefabsByComponents;
    private Dictionary<(int, int), PrefabByComponentData> _cashPrefabsByComponents = new();

    private List<PrefabByComponentData> _cashUpdatePrefabByComponentDatas = new();
    private EntityInProcess _entityInProcess;

    public List<PrefabByComponentData> PrefabsByComponents => _prefabsByComponents;
    public long Id => _entityInProcess.Id;
    public EntityInProcess EntityInProcess => _entityInProcess;
    /// <summary>
    /// not destroy
    /// </summary>
    public bool IsExist => gameObject.activeSelf;
    public string GetTypeKey => TypeKey;

    internal T GetMyComponent<T>(int addingKey = 0) where T : PrefabByComponentData
    {
        InitPBCs();

        int typeName = TypeCache<T>.IdType;

        if (_cashPrefabsByComponents.TryGetValue((typeName, addingKey), out PrefabByComponentData res))
        {
            return res as T;
        }
        return null;
    }

    private void Awake()
    {
        Uid = PoolCounter<EntityMonobeh>.NextUid();
    }

    private void InitPBCs()
    {
        if (_cashPrefabsByComponents.Count == 0)
        {
            _prefabsByComponents.ForEach(p => _cashPrefabsByComponents.Add((p.KeyType, p.AddingKey), p));
        }
    }

    internal void VirtualCreate()
    {
        gameObject.SetActive(true);
    }

    public void Init(EntityInProcess entityInProcess)
    {
        _entityInProcess = entityInProcess;
        transform.position = _entityInProcess.Position;

        entityInProcess.UpdateEIP += UpdateEntity;

        InitComponents();

        _entityInProcess.UpdateEntity();
    }

    private void InitComponents()
    {
        foreach (var c in _entityInProcess.EntityData._cashComponents.Values)
        {
            var pbcs = PrefabsByComponents.GetComponents(c);
            foreach (var p in pbcs)
            {
                p.Init(c, _entityInProcess);
            }
        }
        foreach (var c in _prefabsByComponents)
        {
            if (c._isNeedUpdate)
            {
                _cashUpdatePrefabByComponentDatas.Add(c);
            }
        }
    }

    private void UpdateEntity()
    {
        if (_entityInProcess.EntityIsDeleted)
        {
            WorldViewer.Instance.RemoveEntity(this);
        }
        else
        {
            _cashUpdatePrefabByComponentDatas.ForEach(c => c.UpdateComponent());
        }
    }

    private void OnDestroy()
    {
        _entityInProcess.UpdateEIP -= UpdateEntity;
    }

    internal void VirtualDestroy()
    {
        gameObject.SetActive(false);
        _entityInProcess.UpdateEIP -= UpdateEntity;
//        _entityInProcess = null;
        _cashUpdatePrefabByComponentDatas.Clear();
        _prefabsByComponents.ForEach(c => c.VirtualDestroy());
    }

    #region Config/Backend

    public void UseCommand(EntityData entity, int keyComponent, int addingKey, string keyCommand, string message, WorldData worldData)
    {
        foreach (var c in _prefabsByComponents)
        {
            if (c.KeyType == keyComponent && c.AddingKey == addingKey)
            {
                c.ExecuteCommand(entity, keyCommand, message, worldData);
            }
        }
    }

    public EntityData CreateEntity(float xpos = 0, float zpos = 0)
    {
        var newEntity = new EntityData(xpos, zpos);
        newEntity.TypeKey = TypeKey;

        foreach (var p in _prefabsByComponents)
        {
            newEntity._cashComponents.TryAdd((p.KeyComponentData, p.AddingKey), p.GetComponentData);
        }
        _prefabsByComponents.ForEach(x => x.PrepareEntityBeforeCreate(newEntity));

        return newEntity;
    }

    #endregion

    public override int GetHashCode()
    {
        return Uid;
    }

    public override bool Equals(object obj)
    {
        if (obj is EntityMonobeh other)
        {
            return Uid == other.Uid;
        }
        return false;
    }
}