using UnityEngine;

[System.Serializable]
public class InventoryController
{
    [SerializeField] private UIIconPresent _cursorDragDrop;

    private long _tempIdInv = -1;
    private string _keyTempInventory = string.Empty;
    private int _tempIdSlot = -1;

    private bool IsDragging => _tempIdInv == -1;

    public InventoryController()
    {
        SetEmptyTemp();
    }

    public void ClickSlot(long idEntity, string addingKey, int idSlot)
    {

    }

    public void DragSlot(long idEntity, string addingKey, int idSlot)
    {
        _tempIdInv = idEntity;
        _keyTempInventory = addingKey;
        _tempIdSlot = idSlot;

        var command = PlayerPresent.GetCommandDragItem(_tempIdInv, _keyTempInventory, _tempIdSlot);

        UIPlayerManager.Instance.EntityMonobeh.EntityInProcess.SendCommand(command);
        //send update inv from and ent with hand
    }

    private void SetEmptyTemp()
    {
        _tempIdInv = -1;
        _keyTempInventory = string.Empty;
        _tempIdSlot = -1;
    }

    public string DropSlot(long idEntity, string addingKey, int idSlot)
    {
        TransportMessage mes = new()
        {
            IdEntityInventoryFrom = _tempIdInv,
            KeyInventoryFrom = _keyTempInventory,
            IdSlotInvenotyFrom = _tempIdSlot,
            IdEntityHand = UIPlayerManager.Instance.EntityMonobeh.Id,
            IdEntityInventoryTo = idEntity,
            KeyInventoryTo = addingKey,
            IdSlotInvenotyTo = idSlot,
        };
        
        return JsonUtility.ToJson(mes);
    }

    public void UpdateHandler()
    {
        var itemHand = UIPlayerManager.Instance.EntityMonobeh.EntityInProcess.EntityData.Components.GetComponent<ComponentPlayerId>().ItemHand;

        _cursorDragDrop.gameObject.SetActive(!itemHand.IsEmpty);
        if (!itemHand.IsEmpty)
        {
            UpdateHand(itemHand);
        }
    }

    public void UpdateHand(ItemData itemData)
    {
        _cursorDragDrop.InitIcon(new UIIconModel(itemData));
    }

    public static void PrepareTransportMessage(string message, WorldData worldData, EntityMonobeh prefabForCreateEnt)
    {
        var trMes = JsonUtility.FromJson<TransportMessage>(message);

        EntityData fromEnt = null;
        if (trMes.IdEntityInventoryFrom != -1)
        {
            fromEnt = worldData.GetEntityById(trMes.IdEntityInventoryFrom);
        }
        EntityData toEnt = null;
        if (trMes.IdEntityInventoryTo != -1)
        {
            toEnt = worldData.GetEntityById(trMes.IdEntityInventoryTo);
        }

        ItemData focusItem = null;
        ComponentInventory fromInv = null;
        if (fromEnt != null)
        {
            fromInv = fromEnt.Components.GetComponent<ComponentInventory>(trMes.KeyInventoryFrom);
        }
        EntityData handEnt = worldData.GetEntityById(trMes.IdEntityHand);
        if (trMes.IdEntityHand != -1)
        {
            focusItem = handEnt.Components.GetComponent<ComponentPlayerId>().ItemHand;
        }
        else if (fromEnt != null)
        {
            focusItem = fromInv.Items[trMes.IdSlotInvenotyFrom];
        }
        else
        {
            Debug.LogError($"BLA PIZDEC: {message}");
        }

        var toInv = toEnt.Components.GetComponent<ComponentInventory>(trMes.KeyInventoryTo);
        var leftItem = toInv.AddItem(focusItem, trMes.IdSlotInvenotyTo);
        if (leftItem.Count > 0)
        {
            if (fromInv != null)
            {
                if (fromInv.Items[trMes.IdSlotInvenotyFrom].AvailableSlotForItem(leftItem))
                {
                    fromInv.AddItem(leftItem, trMes.IdSlotInvenotyFrom);
                }
                else
                {
                    toInv.AddItem(leftItem);
                }
            }
            else
            {
                var placeForDrop = toEnt.Position;

                var itemEnt = prefabForCreateEnt.CreateEntity(placeForDrop.x, placeForDrop.z);

                var cmpItem = itemEnt.Components.GetComponent<ComponentItemPresent>();
                cmpItem.SetItem(leftItem);

                worldData.AddEntity(itemEnt);
            }
        }
        focusItem.SetEmpty();

        if (fromEnt != null)
        {
            fromEnt.UpdateEntity();
        }
        if (toEnt != null)
        {
            toEnt.UpdateEntity();
        }
        if (handEnt != null)
        {
            handEnt.UpdateEntity();
        }
    }
}

[System.Serializable]
public class TransportMessage
{
    public long IdEntityHand = -1;
    public long IdEntityInventoryFrom = -1;
    public long IdEntityInventoryTo = -1;
    public string KeyInventoryFrom = string.Empty;
    public string KeyInventoryTo = string.Empty;
    public int IdSlotInvenotyFrom = -1;
    public int IdSlotInvenotyTo = -1;
}