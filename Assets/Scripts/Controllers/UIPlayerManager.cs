using UnityEngine;
using UnityEngine.EventSystems;

public class UIPlayerManager : MonoBehaviour
{
    public static UIPlayerManager Instance;

    [SerializeField] private UIPresentInventory uIPresentInventory;
    [SerializeField] private UIPanelCraftGroups _uIPanelCraftGroups;
    [SerializeField] private UIIconPresent _uIIconPresent;
    [SerializeField] private SetterBuild _setterBuild;
    //    [SerializeField] private State _setPlanBuildState;
    [SerializeField] private GameObject _panelForDropItem;

    private RecipeSO _tempRecipe;
    private EntityMonobeh _entityMonobehPlayer;
    private long _tempIdInv;
    private ItemData _tempFromSlot;//need some remove to data layer

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
        var ci = entity.EntityInProcess.EntityData.Components.GetComponent<ComponentInventory>();

        uIPresentInventory.Init(ci);
        uIPresentInventory.OnDragItem += DragItem;
        uIPresentInventory.OnDropItem += DropItem;
        uIPresentInventory.OnUseItem += UseItemByInventory;

        _uIPanelCraftGroups.Init(ci);
        _uIPanelCraftGroups.OnApplyCraft += UpdateModules;

        UpdateModules();
    }

    private void UpdateModules()
    {
        var itemHand = _entityMonobehPlayer.EntityInProcess.EntityData.Components.GetComponent<ComponentPlayerId>().ItemHand;

        uIPresentInventory.UpdateSlots();

        _uIIconPresent.gameObject.SetActive(!itemHand.IsEmpty);
//        _panelForDropItem.SetActive(!itemHand.IsEmpty);
        if (!itemHand.IsEmpty)
        {
            _uIIconPresent.InitIcon(new UIIconModel(itemHand));
        }
    }

    private void DragItem(long idInv, string idInvKey/*??*/, ItemData dragItem)
    {
        _tempIdInv = idInv;
        _tempFromSlot = dragItem;

        var playerComp = _entityMonobehPlayer.EntityInProcess.EntityData.Components.GetComponent<ComponentPlayerId>();
        playerComp.PickItemByHand(dragItem);
        //send command player pick item and remove item from inventory

        UpdateModules();
    }

    private void DropItem(long idInv, string idInvKey/*??*/, ItemData dropSlot)
    {
        //send command id from and to inventory, id slots, idEnt hand
    }

    private void DragItem(ItemData dragItem)
    {
        _tempFromSlot = dragItem;

        var playerComp = _entityMonobehPlayer.EntityInProcess.EntityData.Components.GetComponent<ComponentPlayerId>();
        playerComp.PickItemByHand(dragItem);

        dragItem.SetEmpty();
        _tempFromSlot.SetEmpty();

        UpdateModules();
    }

    private void DropItem(ItemData dropItem)
    {//logic move to entity
        var playerComp = _entityMonobehPlayer.EntityInProcess.EntityData.Components.GetComponent<ComponentPlayerId>();
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
            var ci = _entityMonobehPlayer.EntityInProcess.EntityData.Components.GetComponent<ComponentInventory>();
            ci.AddItem(itemHand);
        }
        else
        {
            _tempFromSlot.Replace(dropItem);
            dropItem.Replace(itemHand);
        }

        _tempFromSlot = null;
        itemHand.SetEmpty();

        UpdateModules();
    }

    private void UseItemByInventory(int index)
    {
        var com = ComponentInventory.GetCommandUseItem(index);
        com.IdEntity = _entityMonobehPlayer.EntityInProcess.Id;
        _entityMonobehPlayer.EntityInProcess.SendCommand(com);
    }

    public void RunPlanBuild(RecipeSO recipe)
    {
        _tempRecipe = recipe;
        _setterBuild.gameObject.SetActive(true);
        _setterBuild.Init(recipe.IconBuild);
    }

    public void TrySetBuild(Vector3 target)
    {
        var compInv = _entityMonobehPlayer.PrefabsByComponents.GetComponent<InventoryPBCD>();
        var cmdSetter = compInv.GetCommandSetEntity(_entityMonobehPlayer.EntityInProcess.EntityData, _tempRecipe, target);
        _entityMonobehPlayer.EntityInProcess.SendCommand(cmdSetter);

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
        if (IsReadySetBuild && Input.GetMouseButtonDown(1))
        {
            CancelPlanBuild();
        }
    }

    public void DropItem(BaseEventData eventData)
    {
        var itemHand = _entityMonobehPlayer.EntityInProcess.EntityData.Components.GetComponent<ComponentPlayerId>().ItemHand;
        if (!itemHand.IsEmpty)
        {
            var compPBC = _entityMonobehPlayer.PrefabsByComponents.GetComponent<PlayerPresent>();

            var cmdDropItem = compPBC.GetCommandDropItem(_entityMonobehPlayer.EntityInProcess.EntityData);

            _entityMonobehPlayer.EntityInProcess.SendCommand(cmdDropItem);

            _tempFromSlot = null;
            UpdateModules();
        }
    }

    private void LateUpdate()
    {
        if (Input.GetMouseButtonUp(0))// && !MouseOverUI)
        {
            var itemHand = _entityMonobehPlayer.EntityInProcess.EntityData.Components.GetComponent<ComponentPlayerId>().ItemHand;
            if (!itemHand.IsEmpty)
            {
                var compPBC = _entityMonobehPlayer.PrefabsByComponents.GetComponent<PlayerPresent>();

                var cmdDropItem = compPBC.GetCommandDropItem(_entityMonobehPlayer.EntityInProcess.EntityData);

                _entityMonobehPlayer.EntityInProcess.SendCommand(cmdDropItem);

                _tempFromSlot = null;
                UpdateModules();
            }
        }/**/
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