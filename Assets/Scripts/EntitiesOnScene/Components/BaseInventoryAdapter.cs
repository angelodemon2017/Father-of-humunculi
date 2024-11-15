using System;
using System.Collections.Generic;
using UnityEngine;

public class BaseInventoryAdapter : PrefabByComponentData
{
    [SerializeField] private EntityMonobeh _droppedItemPrefab;
    [SerializeField] private List<EnumItemCategory> slots = new();
    [SerializeField] private string _addingKey;

    private EntityInProcess _entityInProcess;
    private ComponentInventory _componentData;
    private List<UIIconPresent> _slots = new();

    public override string KeyComponent => typeof(BaseInventoryAdapter).Name;
    public override string KeyComponentData => typeof(ComponentInventory).Name;

    internal override ComponentData GetComponentData => new ComponentInventory(slots, _addingKey);

    public override void Init(ComponentData componentData, EntityInProcess entityInProcess = null)
    {
        _entityInProcess = entityInProcess;
        _componentData = (ComponentInventory)componentData;
            //entityInProcess.EntityData.Components.GetComponent<ComponentInventory>(_addingKey);
    }

    public override void ExecuteCommand(EntityData entity, string command, string message, WorldData worldData)
    {
        switch (command)
        {
            default:
                break;
        }
    }

    private void DropItem(TransportMessage transportMessage, WorldData worldData)
    {
        var focusItem = worldData.GetEntityById(transportMessage.IdEntityHand).Components.GetComponent<ComponentPlayerId>().ItemHand;

        var toEnt = worldData.GetEntityById(transportMessage.IdEntityInventoryTo);

        var toInv = toEnt.Components.GetComponent<ComponentInventory>(transportMessage.KeyInventoryTo);

        toInv.AddItem(focusItem, transportMessage.IdSlotInvenotyTo);

    }

    public void InitSlots(List<UIIconPresent> slots)
    {
        _slots.Clear();
        foreach (var s in slots)
        {
            s.OnClickIcon += ClickSlot;
            s.OnDragHandler += DragSlot;
            s.OnDropHandler += DropSlot;
        }
        UpdateComponent();
    }

    internal override void UpdateComponent()
    {
        for (int i = 0; i < _componentData.Items.Count; i++)
        {
            UIIconModel iconModel = new UIIconModel(_componentData.Items[i]);
            iconModel.Index = i;
            _slots[i].InitIcon(iconModel);
        }
    }

    private void ClickSlot(int idSlot)
    {
        UIPlayerManager.Instance._inventoryController.ClickSlot(_entityInProcess.Id, AddingKey, idSlot);
    }

    private void DragSlot(int idSlot)
    {
        UIPlayerManager.Instance._inventoryController.DragSlot(_entityInProcess.Id, AddingKey, idSlot);
    }

    private void DropSlot(int idSlot)
    {
         UIPlayerManager.Instance._inventoryController.DropSlot(_entityInProcess.Id, AddingKey, idSlot);
    }

    public static CommandData PickUpItem()
    {
        var transMessage = new TransportMessage();
        var message = JsonUtility.ToJson(transMessage);
        return new CommandData()
        {
            KeyComponent = typeof(BaseInventoryAdapter).Name,
            Message = message,
        };
    }
}