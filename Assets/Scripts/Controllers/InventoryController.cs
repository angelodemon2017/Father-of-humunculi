using UnityEngine;

public class InventoryController
{
    [SerializeField] private UIIconPresent _uIIconPresent;

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

        var command = PlayerPresent.GetCommandDragItem(_tempIdInv, addingKey, _tempIdSlot);

        UIPlayerManager.Instance.EntityMonobeh.EntityInProcess.SendCommand(command);
        //send update inv from and ent with hand
    }

    private void SetEmptyTemp()
    {
        _tempIdInv = -1;
        _keyTempInventory = string.Empty;
        _tempIdSlot = -1;
    }

    public void DropSlot(long idEntity, string addingKey, int idSlot)
    {

//        TransportMessage mes = 

//        var command = PlayerPresent.GetCommandDragItem(_tempIdInv, addingKey, _tempIdSlot);

        //        UIPlayerManager.Instance.EntityMonobeh.EntityInProcess.SendCommand(command);
    }

    public void UpdateHandler()
    {
        var itemHand = UIPlayerManager.Instance.EntityMonobeh.EntityInProcess.EntityData.Components.GetComponent<ComponentPlayerId>().ItemHand;

        _uIIconPresent.gameObject.SetActive(!itemHand.IsEmpty);
        if (!itemHand.IsEmpty)
        {
            UpdateHand(itemHand);
        }
    }

    public void UpdateHand(ItemData itemData)
    {
        _uIIconPresent.InitIcon(new UIIconModel(itemData));
    }

    public static void PrepareTransportMessage(string message, WorldData worldData)
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
        if (trMes.IdEntityHand != -1)
        {
            focusItem = worldData.GetEntityById(trMes.IdEntityHand).Components.GetComponent<ComponentPlayerId>().ItemHand;
        }
        else if (fromEnt != null)
        {
            var fromInv = fromEnt.Components.GetComponent<ComponentInventory>(trMes.KeyInventoryFrom);
            focusItem = fromInv.Items[trMes.IdSlotInvenotyFrom];
        } 
        else
        {
            Debug.LogError($"BLA PIZDEC: {message}");
        }



        if (fromEnt != null)
        {
            fromEnt.UpdateEntity();
        }
        if (toEnt != null)
        {
            toEnt.UpdateEntity();
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