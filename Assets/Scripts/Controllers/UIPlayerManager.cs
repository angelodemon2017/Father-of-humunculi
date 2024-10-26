using UnityEngine;

public class UIPlayerManager : MonoBehaviour
{
    public static UIPlayerManager Instance;

    [SerializeField] private UIPresentInventory uIPresentInventory;
    [SerializeField] private UIPanelCraftGroups _uIPanelCraftGroups;
    [SerializeField] private UIIconPresent _uIIconPresent;
    [SerializeField] private SetterBuild _setterBuild;

    private RecipeSO _tempRecipe;
    private EntityInProcess _entityInProcess;
    private ItemData _tempFromSlot;

    public UIPresentInventory UIPresentInventory => uIPresentInventory;

    private void Awake()
    {
        Instance = this;
        uIPresentInventory.OnComponentUpdated += UpdateModules;
    }

    public void InitEntity(EntityInProcess entity)
    {
        _entityInProcess = entity;

        _entityInProcess.UpdateEIP += UpdateModules;

        //TODO cycle init all components
        var ci = entity.EntityData.Components.GetComponent<ComponentInventory>();

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
        var ent = _entityInProcess.EntityData as EntityPlayer;

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
        var ent = _entityInProcess.EntityData as EntityPlayer;
        ent.PickItemByHand(dragItem);

        _tempFromSlot.SetEmpty();

        UpdateModules();
    }

    private void DropItem(ItemData dropItem)
    {//logic move to entity
        var ent = _entityInProcess.EntityData as EntityPlayer;
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
        com.IdEntity = _entityInProcess.Id;
        _entityInProcess.SendCommand(com);
    }

    public void RunPlanBuild(RecipeSO recipe)
    {
        _tempRecipe = recipe;
        _setterBuild.gameObject.SetActive(true);
        _setterBuild.Init(recipe.IconBuild);
    }

    private void CancelPlanBuild()
    {
        _tempRecipe = null;
        _setterBuild.gameObject.SetActive(false);
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
    }

    private void LateUpdate()
    {
        if (Input.GetMouseButtonUp(0))
        {
            var ent = _entityInProcess.EntityData as EntityPlayer;
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
        _entityInProcess.UpdateEIP -= UpdateModules;

        uIPresentInventory.OnComponentUpdated -= UpdateModules;
        uIPresentInventory.OnDragItem -= DragItem;
        uIPresentInventory.OnDropItem -= DropItem;
        uIPresentInventory.OnUseItem -= UseItemByInventory;

        _uIPanelCraftGroups.OnApplyCraft -= UpdateModules;
    }
}