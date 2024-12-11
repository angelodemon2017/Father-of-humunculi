using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EntityMonobeh : MonoBehaviour
{
    [SerializeField] private string TypeKey;
    [SerializeField] private List<PrefabByComponentData> _prefabsByComponents;
    private Dictionary<(string, string), PrefabByComponentData> _cashPrefabsByComponents = new();

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

    internal T GetMyComponent<T>(string addingKey = "") where T : PrefabByComponentData
    {
        InitPBCs();

        if (_cashPrefabsByComponents.TryGetValue((typeof(T).Name, addingKey), out PrefabByComponentData res))
        {
            return (T)res;
        }
        else
        {
            return null;
        }
    }
    private void InitPBCs()
    {
        if (_cashPrefabsByComponents.Count == 0)
        {
            _prefabsByComponents.ForEach(p => _cashPrefabsByComponents.Add((p.KeyComponent, p.AddingKey), p));
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
        foreach (var c in _entityInProcess.EntityData.Components)
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

    public void UseCommand(EntityData entity, string keyComponent, string addingKey, string keyCommand, string message, WorldData worldData)
    {
        foreach (var c in _prefabsByComponents)
        {
            if (c.KeyComponent == keyComponent && c.AddingKey == addingKey)
            {
                c.ExecuteCommand(entity, keyCommand, message, worldData);
            }
        }
    }

    public EntityData CreateEntity(float xpos = 0, float zpos = 0)
    {
        var newEntity = new EntityData(xpos, zpos);
        newEntity.TypeKey = TypeKey;// gameObject.name;

        foreach (var p in _prefabsByComponents)
        {
            if (!newEntity.Components.Any(c => c.KeyName == p.KeyComponentData && c.AddingKey == p.AddingKey))
            {
                newEntity.Components.Add(p.GetComponentData);
            }
        }
        _prefabsByComponents.ForEach(x => x.PrepareEntityBeforeCreate(newEntity));

        return newEntity;
    }

    #endregion
}