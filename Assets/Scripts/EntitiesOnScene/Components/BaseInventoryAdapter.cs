using System.Collections.Generic;
using UnityEngine;

public class BaseInventoryAdapter : PrefabByComponentData
{
    [SerializeField] private List<EnumItemCategory> slots = new();
    [SerializeField] private string _addingKey;

    private ComponentInventory _componentData;

    public override string KeyComponent => typeof(SpawnerByCounter).Name;
    public override string KeyComponentData => typeof(ComponentInventory).Name;

    internal override ComponentData GetComponentData => new ComponentInventory(slots, _addingKey);

    public override void Init(ComponentData componentData, EntityInProcess entityInProcess = null)
    {
        _componentData = entityInProcess.EntityData.Components.GetComponent<ComponentInventory>(_addingKey);
    }

    public override void ExecuteCommand(EntityData entity, string command, string message, WorldData worldData)
    {
        switch (command)
        {
            default:
                break;
        }
    }

    public static CommandData PickUpItem()
    {
        return new CommandData();
    }
}