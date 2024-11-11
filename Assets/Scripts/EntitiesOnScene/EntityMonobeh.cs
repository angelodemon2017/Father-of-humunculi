using System.Collections.Generic;
using UnityEngine;

public class EntityMonobeh : MonoBehaviour
{
    [SerializeField] private List<PrefabByComponentData> _prefabsByComponents;

    private EntityInProcess _entityInProcess;

    public List<PrefabByComponentData> PrefabsByComponents => _prefabsByComponents;
    public long Id => _entityInProcess.Id;
    public EntityInProcess EntityInProcess => _entityInProcess;

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
            var pbc = PrefabsByComponents.GetComponent(c);
            if (pbc != null)
            {
                pbc.Init(c, _entityInProcess);
            }
        }

//        _entityInProcess.EntityData.Components.ForEach(c => PrefabsByComponents.GetComponent(c).Init(c));
    }

    private void UpdateEntity()
    {
        if (_entityInProcess.EntityIsDeleted)
        {
            GameProcess.Instance.RemoveEIP(_entityInProcess);
            WorldViewer.Instance.RemoveEntity(this);
        }
    }

    private void OnDestroy()
    {
        _entityInProcess.UpdateEIP -= UpdateEntity;
    }

    public EntityData CreateEntity(float xpos = 0, float zpos = 0)
    {
        var newEntity = new EntityData(xpos, zpos);
        newEntity.TypeKey = gameObject.name;

        foreach (var c in _prefabsByComponents)
        {
            newEntity.Components.Add(c.GetComponentData);
        }

        return newEntity;
    }

    public void SendCommand(CommandData comand)
    {
        _entityInProcess.SendCommand(comand);
    }

    public void UseCommand(EntityData entity, string keyCommand, string message, WorldData worldData)
    {
        foreach (var c in _prefabsByComponents)
        {
            if (c.KeyComponent == keyCommand)
            {
                c.ExecuteCommand(entity, message, worldData);
            }
        }
    }
}