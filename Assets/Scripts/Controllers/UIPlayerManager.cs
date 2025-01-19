using UnityEngine;
using UnityEngine.EventSystems;

public class UIPlayerManager : MonoBehaviour
{
    public static UIPlayerManager Instance;

    [SerializeField] private DebugPanelUI _debugPanelUI;
    [SerializeField] private UIPresentInventory uIPresentInventory;
    [SerializeField] private UIPanelCraftGroups _uIPanelCraftGroups;
    [SerializeField] private UIPresentHunger _uIPresentHunger;
    [SerializeField] private UIEquipmentView _uIEquipmentView;
    [SerializeField] private SetterBuild _setterBuild;
    [SerializeField] private GameObject _panelForDropItem;

    private RecipeSO _tempRecipe;
    public InventoryController _inventoryController = new();
    private EntityMonobeh _entityMonobehPlayer;
    public static bool ISCURSORUNDERUI;

    public EntityMonobeh EntityMonobeh => _entityMonobehPlayer;
    public bool IsReadySetBuild => _tempRecipe != null;
    public bool MouseOverUI => EventSystem.current.IsPointerOverGameObject();

    private void Awake()
    {
        Instance = this;
        uIPresentInventory.OnComponentUpdated += UpdateModules;
        CancelPlanBuild();
    }

    public void InitEntity(EntityMonobeh entity)
    {
        _entityMonobehPlayer = entity;

        _entityMonobehPlayer.EntityInProcess.UpdateEIP += UpdateModules;

        //TODO cycle init all components
        var ci = entity.EntityInProcess.EntityData.GetComponent<ComponentInventory>(0);//may be make enum

        uIPresentInventory.Init(ci, entity.EntityInProcess);
        uIPresentInventory.OnUseItem += UseItemByInventory;

        _uIPanelCraftGroups.Init(_entityMonobehPlayer);
        _uIPanelCraftGroups.OnApplyCraft += UpdateModules;

        var ci2 = entity.EntityInProcess.EntityData.GetComponent<ComponentInventory>(1);//may be make enum
        _uIEquipmentView.Init(ci2, entity.EntityInProcess);

        var ch = entity.EntityInProcess.EntityData.GetComponent<ComponentHunger>();
        _uIPresentHunger.Init(ch, entity.EntityInProcess);

        var chp = entity.EntityInProcess.EntityData.GetComponent<ComponentHPData>();
        _uIPresentHunger.InitHP(chp, entity.EntityInProcess);

        UpdateModules();
    }

    private void UpdateModules()
    {
        _inventoryController.UpdateHandler();
    }

/*    private void DragItem(long idInv, string idInvKey, ItemData dragItem)
    {
        var playerComp = _entityMonobehPlayer.EntityInProcess.EntityData.GetComponent<ComponentPlayerId>();
        playerComp.PickItemByHand(dragItem);

        UpdateModules();
    }

    private void DropItem(long idInv, string idInvKey, ItemData dropSlot)
    {
        //send command id from and to inventory, id slots, idEnt hand
    }/**/

    private void DragItem(ItemData dragItem)
    {
        _inventoryController.UpdateHand(dragItem);
        dragItem.SetEmpty();
    }

    private void DropItem(ItemData dropItem)
    {//logic move to entity
        var playerComp = _entityMonobehPlayer.EntityInProcess.EntityData.GetComponent<ComponentPlayerId>();
        var itemHand = playerComp.ItemHand;

        if (itemHand.IsEmpty)
        {
            return;
        }

        if (dropItem.IsEmpty)
        {
            dropItem.Replace(itemHand);
        }
        else if (itemHand.Id == dropItem.Id)
        {
            itemHand.Count = dropItem.TryAdd(itemHand);
            var ci = _entityMonobehPlayer.EntityInProcess.EntityData.GetComponent<ComponentInventory>();
            ci.AddItem(itemHand);
        }
        else
        {
            dropItem.Replace(itemHand);
        }

        itemHand.SetEmpty();

        UpdateModules();
    }

    private void UseItemByInventory(int index)
    {
        var com = ComponentInventory.GetCommandUseItem(index);
        com.IdEntity = _entityMonobehPlayer.EntityInProcess.Id;
        _entityMonobehPlayer.EntityInProcess.SendCommand(com);
    }

    public void RunPlanBuild(RecipeEntitySpawn recipe)
    {
        _tempRecipe = recipe;
        _setterBuild.gameObject.SetActive(true);
        _setterBuild.Init(recipe.IconBuild);
    }

    public void TrySetBuild(Vector3 target)
    {
        if (!_tempRecipe.AvailableRecipe(_entityMonobehPlayer.EntityInProcess.EntityData))
        {
            return;
        }

        _tempRecipe.RunCraftState(target);

        CancelPlanBuild();
    }

    public void HideCursorBuild()
    {
        _setterBuild.gameObject.SetActive(false);
    }

    private void CancelPlanBuild()
    {
        _tempRecipe = null;
        HideCursorBuild();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            UseItemByInventory(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            UseItemByInventory(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            UseItemByInventory(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            UseItemByInventory(3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            UseItemByInventory(4);
        }
        if (Input.GetKeyDown(KeyCode.F3))
        {
            _debugPanelUI.gameObject.SetActive(!_debugPanelUI.gameObject.activeSelf);
        }
        if (IsReadySetBuild && Input.GetMouseButtonDown(1))
        {
            CancelPlanBuild();
        }
    }

    private void LateUpdate()
    {
        if (Input.GetMouseButtonUp(0))
        {
            var itemHand = _entityMonobehPlayer.EntityInProcess.EntityData.GetComponent<ComponentPlayerId>().ItemHand;
            if (!itemHand.IsEmpty)
            {
                var compPBC = _entityMonobehPlayer.GetMyComponent<PlayerPresent>();

                var cmdDropItem = compPBC.GetCommandDropItem(_entityMonobehPlayer.EntityInProcess.EntityData);

                _entityMonobehPlayer.EntityInProcess.SendCommand(cmdDropItem);

                UpdateModules();
            }
        }
    }

    private void OnDestroy()
    {
        _entityMonobehPlayer.EntityInProcess.UpdateEIP -= UpdateModules;

        uIPresentInventory.OnComponentUpdated -= UpdateModules;
        uIPresentInventory.OnDragItem -= DragItem;
        uIPresentInventory.OnDropItem -= DropItem;
        uIPresentInventory.OnUseItem -= UseItemByInventory;

        _uIPanelCraftGroups.OnApplyCraft -= UpdateModules;
    }
}