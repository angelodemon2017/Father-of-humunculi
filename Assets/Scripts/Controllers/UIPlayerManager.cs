using UnityEngine;

public class UIPlayerManager : MonoBehaviour
{
    public static UIPlayerManager Instance;

    [SerializeField] private UIPresentInventory uIPresentInventory;
    [SerializeField] private UIPanelCraftGroups _uIPanelCraftGroups;

    private EntityInProcess _entityInProcess;
//    private ItemData _handlerTempData;
    private ItemData _tempFromSlot;

    public UIPresentInventory UIPresentInventory => uIPresentInventory;

    private void Awake()
    {
        Instance = this;
        uIPresentInventory.ComponentUpdated += UpdateModules;
    }

    public void InitEntity(EntityInProcess entity)
    {
        _entityInProcess = entity;

        _entityInProcess.UpdateEIP += UpdateModules;

        //TODO cycle init all components
        var ci = entity.EntityData.Components.GetComponent<ComponentInventory>();
        uIPresentInventory.Init(ci);
        _uIPanelCraftGroups.Init(ci);
        _uIPanelCraftGroups.OnApplyCraft += UpdateModules;

        UpdateModules();
    }

    private void UpdateModules()
    {
        uIPresentInventory.UpdateSlots();
    }

    private void DragItem(ItemData dragItem)
    {
        _tempFromSlot = dragItem;
        var ent = _entityInProcess.EntityData as EntityPlayer;
        ent.PickItemByHand(dragItem);

//        _handlerTempData = new ItemData(dragItem);
        _tempFromSlot.SetEmpty();

        UpdateModules();
    }

    private void DropItem(ItemData dropItem)
    {
        if (_tempFromSlot.EnumId == EnumItem.None)
        {
            return;
        }

        if (_tempFromSlot.EnumId == dropItem.EnumId)
        {//connect

        }
        else
        {//replace

        }

        dropItem.SetEmpty();
//        _handlerTempData.SetEmpty();

        UpdateModules();
    }

    private void OnDestroy()
    {
        _entityInProcess.UpdateEIP -= UpdateModules;
        _uIPanelCraftGroups.OnApplyCraft -= UpdateModules;
        uIPresentInventory.ComponentUpdated -= UpdateModules;
    }
}