using UnityEngine;

public class UIPlayerManager : MonoBehaviour
{
    public static UIPlayerManager Instance;

    [SerializeField] private UIPresentInventory uIPresentInventory;
    [SerializeField] private UIPanelCraftGroups _uIPanelCraftGroups;
    [SerializeField] private UIIconPresent _uIIconPresent;
    [SerializeField] private SetterBuild _setterBuild;
//    [SerializeField] private State _setPlanBuildState;

    private RecipeSO _tempRecipe;
    private EntityMonobeh _entityMonobeh;
    private ItemData _tempFromSlot;

    public EntityMonobeh EntityMonobeh => _entityMonobeh;
    public bool IsReadySetBuild => _tempRecipe != null;
    public UIPresentInventory UIPresentInventory => uIPresentInventory;

    private void Awake()
    {
        Instance = this;
        uIPresentInventory.OnComponentUpdated += UpdateModules;
        CancelPlanBuild();
    }

    public void InitEntity(EntityMonobeh entity)
    {
        _entityMonobeh = entity;

        _entityMonobeh.EntityInProcess.UpdateEIP += UpdateModules;

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
        var ent = _entityMonobeh.EntityInProcess.EntityData as EntityPlayer;

        uIPresentInventory.UpdateSlots();

        _uIIconPresent.gameObject.SetActive(!ent.ItemHand.IsEmpty);
        if (!ent.ItemHand.IsEmpty)
        {
            _uIIconPresent.InitIcon(new UIIconModel(ent.ItemHand));
        }
    }

    private void DragItem(ItemData dragItem)
    {
        _tempFromSlot = dragItem;
        var ent = _entityMonobeh.EntityInProcess.EntityData as EntityPlayer;
        ent.PickItemByHand(dragItem);

        _tempFromSlot.SetEmpty();

        UpdateModules();
    }

    private void DropItem(ItemData dropItem)
    {//logic move to entity
        var ent = _entityMonobeh.EntityInProcess.EntityData as EntityPlayer;
        var itemHand = ent.ItemHand;

        if (itemHand.IsEmpty)
        {
            return;
        }

        if (dropItem.IsEmpty)
        {
            dropItem.Replace(itemHand);
        }
        else if (itemHand.EnumId == dropItem.EnumId)
        {
            itemHand.Count = dropItem.TryAdd(itemHand);
            var ci = ent.Components.GetComponent<ComponentInventory>();
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
        com.IdEntity = _entityMonobeh.EntityInProcess.Id;
        _entityMonobeh.EntityInProcess.SendCommand(com);
    }

    public void RunPlanBuild(RecipeSO recipe)
    {
        _tempRecipe = recipe;
        _setterBuild.gameObject.SetActive(true);
        _setterBuild.Init(recipe.IconBuild);
    }

    public void TrySetBuild(Vector3 target)
    {
        _entityMonobeh.EntityInProcess.SendCommand(
            new CommandData(_entityMonobeh.EntityInProcess.EntityData,
            _tempRecipe, target));
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

    private void LateUpdate()
    {
        if (Input.GetMouseButtonUp(0))
        {
            var ent = _entityMonobeh.EntityInProcess.EntityData as EntityPlayer;
            var itemHand = ent.ItemHand;
            if (!itemHand.IsEmpty)
            {                
                GameProcess.Instance.GameWorld.AddEntity(new EntityItem(itemHand, ent.Position.x, ent.Position.z));
                itemHand.SetEmpty();
                _tempFromSlot = null;

                UpdateModules();
            }
        }
    }

    private void OnDestroy()
    {
        _entityMonobeh.EntityInProcess.UpdateEIP -= UpdateModules;

        uIPresentInventory.OnComponentUpdated -= UpdateModules;
        uIPresentInventory.OnDragItem -= DragItem;
        uIPresentInventory.OnDropItem -= DropItem;
        uIPresentInventory.OnUseItem -= UseItemByInventory;

        _uIPanelCraftGroups.OnApplyCraft -= UpdateModules;
    }
}