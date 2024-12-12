using System.Collections.Generic;
using UnityEngine;
using static OptimazeExtensions;

public class CanvasUIInventoryChest : PrefabByComponentData
{
    public override int KeyType => TypeCache<CanvasUIInventoryChest>.IdType;
    [SerializeField] private UIIconPresent _uiIconPresentPrefab;
    [SerializeField] private Transform _parentSlots;
    [SerializeField] private List<ItemConfig> _startItems;
    [SerializeField] private BaseInventoryAdapter _baseInventoryAdapter;

    private ComponentInventory _component;
    private EntityInProcess _entityInProcess;
    private List<UIIconPresent> _tempSlots = new();

    internal override int AddingKey => _baseInventoryAdapter.AddingKey;
    internal override bool _isNeedUpdate => true;
    public override int KeyComponentData => TypeCache<ComponentInventory>.IdType;

    internal override void PrepareEntityBeforeCreate(EntityData entityData)
    {
        var invs = entityData.GetComponents<ComponentInventory>();

        _startItems.ForEach(i => TryAddItem(invs, new ItemData(i)));
    }

    private void TryAddItem(List<ComponentInventory> invs, ItemData itemData)
    {//TODO big quest, why List<ComponentInventory> invs... need think...
        foreach (ComponentInventory i in invs)
        {
            if (i.AvailableAddItem(itemData))
            {
                i.AddItem(itemData);
                return;
            }
        }
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

            _tempSlots.Add(uicp);
        }
    }
}