using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasUIInventoryChest : PrefabByComponentData
{
    private const char splitter = '^';

    [SerializeField] private GameObject _root;
    [SerializeField] private Button _buttonClose;
    [SerializeField] private UIIconPresent _uiIconPresentPrefab;
    [SerializeField] private int _maxItems;
    [SerializeField] private Transform _parentSlots;
    [SerializeField] private List<ItemConfig> _startItems;

    private ComponentInventory _component;
    private EntityInProcess _entityInProcess;
    private List<UIIconPresent> _tempSlots = new();
    private Transform _whoOpened = null;

    internal override bool _isNeedUpdate => true;
    public override string KeyComponent => typeof(CanvasUIInventoryChest).Name;
    public override string KeyComponentData => typeof(ComponentInventory).Name;
    internal override ComponentData GetComponentData => GenComponent();

    private ComponentInventory GenComponent()
    {
        var newComp = new ComponentInventory(_maxItems);

        _startItems.ForEach(i => newComp.AddItem(new ItemData(i)));

        return newComp;
    }

    public override void Init(ComponentData componentData, EntityInProcess entityInProcess = null)
    {
        _root.SetActive(false);
        _buttonClose.onClick.AddListener(Close);
        _component = (ComponentInventory)componentData;
        _entityInProcess = entityInProcess;
    }

    internal override void UpdateComponent()
    {
        IsOpened();
        _tempSlots.Clear();
        _parentSlots.DestroyChildrens();

        for (int i = 0; i < _component.Items.Count; i++)
        {
            var uicp = Instantiate(_uiIconPresentPrefab, _parentSlots);

            UIIconModel iconModel = new UIIconModel(_component.Items[i]);
            iconModel.Index = i;

            uicp.InitIcon(iconModel);
            uicp.OnClickIconByEntity += ClickOnSlot;
            uicp.OnDragHandlerByEntity += DragSlot;
            uicp.OnDropHandlerByEntity += DropSlot;

            _tempSlots.Add(uicp);
        }
    }

    public override void ExecuteCommand(EntityData entity, string command, string message, WorldData worldData)
    {
        switch (command)
        {
            case Dict.Commands.CloseUI:
                CloseUIData(entity);
                break;
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

    public void PickEntity(EntityData entity, string command, string message, WorldData worldData)
    {
        var compPlayer = entity.Components.GetComponent<ComponentInventory>();

        if (compPlayer != null)
        {
            compPlayer.SetEntityOpener(long.Parse(message));
            entity.UpdateEntity();
        }
    }

    private void CloseUIData(EntityData entity)
    {
        var compPlayer = entity.Components.GetComponent<ComponentInventory>();
        compPlayer.SetEntityOpener(-1);
        entity.UpdateEntity();
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

            playerComp.ItemHand.SetEmpty();

            clickedEntity.UpdateEntity();
            entity.UpdateEntity();
        }
    }

    private void IsOpened()
    {
        var isOpen = _component.WhoOpened == UIPlayerManager.Instance.EntityMonobeh.Id;
        _root.SetActive(isOpen);
        if (_root.activeSelf)
        {
            _whoOpened = UIPlayerManager.Instance.EntityMonobeh.transform;
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

    private void Close()
    {
        _root.SetActive(false);
        _entityInProcess.SendCommand(GetCommandCloseUI());
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

    private CommandData GetCommandCloseUI()
    {
        return new CommandData()
        {
            KeyCommand = Dict.Commands.CloseUI,
            KeyComponent = KeyComponent,
        };
    }

    private void FixedUpdate()
    {
        if (_root.activeSelf)
        {
            if (Vector3.Distance(transform.position, _whoOpened.position) > Config.CloseUIDistance)
            {
                Close();
            }
        }
    }

    private void OnDestroy()
    {
        
    }
}