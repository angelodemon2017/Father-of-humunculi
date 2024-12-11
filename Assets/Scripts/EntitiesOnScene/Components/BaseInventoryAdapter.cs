using System.Collections.Generic;
using UnityEngine;

public class BaseInventoryAdapter : PrefabByComponentData
{
    private const char splitter = '^';

    [SerializeField] private EntityMonobeh _droppedItemPrefab;
    [SerializeField] private List<EnumItemCategory> defaulthSlots = new();
    [SerializeField] private string _addingKey;

    private EntityInProcess _entityInProcess;
    private ComponentInventory _componentData;
    private List<UIIconPresent> _slots = new();

    internal override string AddingKey => _addingKey;
    public override string KeyComponentData => typeof(ComponentInventory).Name;

    internal override ComponentData GetComponentData => defaulthSlots.Count > 0 ? new ComponentInventory(defaulthSlots, _addingKey) : base.GetComponentData;
    //    internal override ComponentData GetComponentData =>  new ComponentInventory(defaulthSlots, _addingKey);

    public override void Init(ComponentData componentData, EntityInProcess entityInProcess = null)
    {
        _entityInProcess = entityInProcess;
        _entityInProcess.UpdateEIP += UpdateComponent;
        _componentData = (ComponentInventory)componentData;
    }

    public override void ExecuteCommand(EntityData entity, string command, string message, WorldData worldData)
    {
        switch (command)
        {
            case Dict.Commands.SlotDrop:
                InventoryController.PrepareTransportMessage(message, worldData, _droppedItemPrefab);
                break;
            case Dict.Commands.UseRecipe:
                UseRecipe(entity, message, worldData);
                break;
            case Dict.Commands.SplitSlot:
                SplitSlot(entity, message);
                break;
            default:
                break;
        }
    }

    public void InitSlots(List<UIIconPresent> slots)
    {
        _slots.Clear();
        foreach (var s in slots)
        {            
            s.OnClickIcon += ClickSlot;
            s.OnDragHandler += DragSlot;
            s.OnDropHandler += DropSlot;
            s.OnClickMBM += MMB;
            _slots.Add(s);
        }
        UpdateComponent();
    }

    internal override void UpdateComponent()
    {
        UpdateSlots();
    }

    private void UpdateSlots()
    {
        if (_slots.Count <= 0 || _componentData == null)
        {
            return;
        }
        for (int i = 0; i < _componentData.Items.Count; i++)
        {
            UIIconModel iconModel = new UIIconModel(_componentData.Items[i]);
            iconModel.Index = i;
            _slots[i].InitIcon(iconModel);
        }
    }

    private void ClickSlot(int idSlot)
    {
        UIPlayerManager.Instance._inventoryController.ClickSlot(_entityInProcess.Id, _addingKey, idSlot);
    }

    private void MMB(int idSlot)
    {
        var com = GetCommandSplitSlot(_entityInProcess.Id, idSlot);
        _entityInProcess.SendCommand(com);
    }

    private void DragSlot(int idSlot)
    {
        UIPlayerManager.Instance._inventoryController.DragSlot(_entityInProcess.Id, _addingKey, idSlot);
    }

    private void DropSlot(int idSlot)
    {
        var mes = UIPlayerManager.Instance._inventoryController.DropSlot(_entityInProcess.Id, _addingKey, idSlot);
        var com = GetCommandDropSlot(_entityInProcess.Id, AddingKey, mes);
        _entityInProcess.SendCommand(com);
    }

    private void SplitSlot(EntityData entity, string message)
    {
        var compInv = entity.GetComponent<ComponentInventory>(AddingKey);
        var idSlot = int.Parse(message);
        compInv.SplitSlot(idSlot);

        entity.UpdateEntity();
    }

    private void UseRecipe(EntityData entity, string message, WorldData worldData)
    {
        var mess = message.Split(splitter);
        RecipeSO recipe = RecipesController.GetRecipe(int.Parse(mess[0]));
        if (recipe.AvailableRecipe(entity))
        {
            recipe.UseAndReleaseRecipe(entity, message);
            entity.UpdateEntity();
        }
    }

    public CommandData GetCommandUseRecipe(EntityData entityData, RecipeSO recipe, Vector3 position)
    {
        return new CommandData()
        {
            IdEntity = entityData.Id,
            KeyComponent = KeyComponent,
            AddingKeyComponent = AddingKey,
            KeyCommand = Dict.Commands.UseRecipe,
            Message = $"{recipe.Index}{splitter}{position.x}{splitter}{position.z}",
        };
    }

    public static CommandData GetCommandDropSlot(long idEnt, string addingKey, string mess)
    {
        return new CommandData()
        {
            IdEntity = idEnt,
            KeyComponent = typeof(BaseInventoryAdapter).Name,
            AddingKeyComponent = addingKey,
            KeyCommand = Dict.Commands.SlotDrop,
            Message = mess,
        };
    }

    public CommandData GetCommandSplitSlot(long idEnt, int slot)
    {
        return new CommandData()
        {
            IdEntity = idEnt,
            KeyComponent = KeyComponent,
            AddingKeyComponent = AddingKey,
            KeyCommand = Dict.Commands.SplitSlot,
            Message = $"{slot}",
        };
    }

    private void OnDestroy()
    {
        _entityInProcess.UpdateEIP -= UpdateComponent;
        _slots.Clear();
    }
}