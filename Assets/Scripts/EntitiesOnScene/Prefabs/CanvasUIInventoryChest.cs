using System.Collections.Generic;
using UnityEngine;

public class CanvasUIInventoryChest : PrefabByComponentData
{
    private const char splitter = '^';

    [SerializeField] private UIIconPresent _uiIconPresentPrefab;
    [SerializeField] private ItemConfig _emptyItem;
    [SerializeField] private int _maxItems;
    [SerializeField] private string _addingKey;
    [SerializeField] private Transform _parentSlots;
    [SerializeField] private List<ItemConfig> _startItems;
    [SerializeField] private BaseInventoryAdapter _baseInventoryAdapter;

    private ComponentInventory _component;
    private EntityInProcess _entityInProcess;
    private List<UIIconPresent> _tempSlots = new();

    internal override string AddingKey => _baseInventoryAdapter.AddingKey;
    internal override bool _isNeedUpdate => true;
    public override string KeyComponent => typeof(CanvasUIInventoryChest).Name;
    public override string KeyComponentData => typeof(ComponentInventory).Name;
    internal override ComponentData GetComponentData => GenComponent();

    private ComponentInventory GenComponent()
    {
        var newComp = new ComponentInventory(_maxItems);
        newComp.AddingKey = AddingKey;
        _startItems.ForEach(i => newComp.AddItem(new ItemData(i)));

        return newComp;
    }

    public override void Init(ComponentData componentData, EntityInProcess entityInProcess = null)
    {
        _component = (ComponentInventory)componentData;
        _entityInProcess = entityInProcess;

        InitSlots();
        _baseInventoryAdapter.Init(_component, _entityInProcess);
        _baseInventoryAdapter.InitSlots(_tempSlots);
    }

    private void InitSlots()
    {
        _tempSlots.Clear();
        _parentSlots.DestroyChildrens();
        for (int i = 0; i < _component.MaxItems; i++)
        {
            var uicp = Instantiate(_uiIconPresentPrefab, _parentSlots);

//            UIIconModel iconModel = new UIIconModel(new ItemData(_emptyItem));
//            iconModel.Index = i;

//            uicp.InitIcon(iconModel);
//            uicp.OnClickIconByEntity += ClickOnSlot;
//            uicp.OnDragHandlerByEntity += DragSlot;
//            uicp.OnDropHandlerByEntity += DropSlot;

            _tempSlots.Add(uicp);
        }
    }

    internal override void UpdateComponent()
    {
/*        for (int i = 0; i < _component.Items.Count; i++)
        {
            UIIconModel iconModel = new UIIconModel(_component.Items[i]);
            iconModel.Index = i;
            _tempSlots[i].InitIcon(iconModel);
        }/**/
    }

    public override void ExecuteCommand(EntityData entity, string command, string message, WorldData worldData)
    {
        switch (command)
        {
            case Dict.Commands.SlotClick:
                ClickSlotByEntity(entity, message, worldData);
                break;
            case Dict.Commands.SlotDrag:
                DragSlotByEntity(entity, message, worldData);
                break;
            case Dict.Commands.SlotDrop:
                DropSlotByEntity(entity, message, worldData);
                break;
            default:
                break;
        }
    }

    private void ClickSlotByEntity(EntityData entity, string message, WorldData worldData)
    {
        var chestInventory = entity.Components.GetComponent<ComponentInventory>();

        var args = message.Split(splitter);

        var clickedEntity = worldData.GetEntityById(long.Parse(args[1]));
        var clickedInventory = clickedEntity.Components.GetComponent<ComponentInventory>();

        if (chestInventory != null && clickedInventory != null)
        {
            var targetSlot = chestInventory.Items[int.Parse(args[0])];

            clickedInventory.AddItem(targetSlot);
            targetSlot.SetEmpty();

            clickedEntity.UpdateEntity();
            entity.UpdateEntity();
        }
    }

    private void DragSlotByEntity(EntityData entity, string message, WorldData worldData)
    {
        var chestInventory = entity.Components.GetComponent<ComponentInventory>();

        var args = message.Split(splitter);
        var targetSlot = chestInventory.Items[int.Parse(args[0])];
        if (targetSlot.IsEmpty)
        {
            return;
        }

        var clickedEntity = worldData.GetEntityById(long.Parse(args[1]));
        var playerComp = clickedEntity.Components.GetComponent<ComponentPlayerId>();

        if (chestInventory != null && playerComp != null)
        {
            playerComp.PickItemByHand(targetSlot);
            targetSlot.SetEmpty();

            clickedEntity.UpdateEntity();
            entity.UpdateEntity();
        }
    }

    private void DropSlotByEntity(EntityData entity, string message, WorldData worldData)
    {
        var chestInventory = entity.Components.GetComponent<ComponentInventory>();

        var args = message.Split(splitter);

        var clickedEntity = worldData.GetEntityById(long.Parse(args[1]));
        var playerComp = clickedEntity.Components.GetComponent<ComponentPlayerId>();
        if (playerComp.ItemHand.IsEmpty)
        {
            return;
        }

        if (chestInventory != null && playerComp != null)
        {
            chestInventory.AddItem(playerComp.ItemHand);

            Debug.Log($"CanvasChest.SetEmpty");
            playerComp.ItemHand.SetEmpty();

            clickedEntity.UpdateEntity();
            entity.UpdateEntity();
        }
    }

    private void ClickOnSlot(int i, long idEntity)
    {
        _entityInProcess.SendCommand(GetCommandActionBySlot(i, idEntity, Dict.Commands.SlotClick));
    }

    private void DragSlot(int i, long idEntity)
    {
        _entityInProcess.SendCommand(GetCommandActionBySlot(i, idEntity, Dict.Commands.SlotDrag));
    }

    private void DropSlot(int i, long idEntity)
    {
        _entityInProcess.SendCommand(GetCommandActionBySlot(i, idEntity, Dict.Commands.SlotDrop));
    }

    private CommandData GetCommandActionBySlot(int idSlot, long whoClick, string command)
    {
        return new CommandData()
        {
            KeyCommand = command,
            KeyComponent = KeyComponent,
            Message = $"{idSlot}{splitter}{whoClick}",
        };
    }
}