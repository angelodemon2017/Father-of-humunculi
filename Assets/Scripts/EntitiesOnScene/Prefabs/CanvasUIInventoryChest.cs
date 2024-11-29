using System.Collections.Generic;
using UnityEngine;

public class CanvasUIInventoryChest : PrefabByComponentData
{
    [SerializeField] private UIIconPresent _uiIconPresentPrefab;
    [SerializeField] private Transform _parentSlots;
    [SerializeField] private List<ItemConfig> _startItems;
    [SerializeField] private BaseInventoryAdapter _baseInventoryAdapter;

    private ComponentInventory _component;
    private EntityInProcess _entityInProcess;
    private List<UIIconPresent> _tempSlots = new();

    internal override string AddingKey => _baseInventoryAdapter.AddingKey;
    internal override bool _isNeedUpdate => true;
    public override string KeyComponentData => typeof(ComponentInventory).Name;
    internal override ComponentData GetComponentData => new ComponentData();

    internal override void PrepareEntityBeforeCreate(EntityData entityData)
    {
        var invs = entityData.Components.GetComponents(typeof(ComponentInventory).Name);

        _startItems.ForEach(i => TryAddItem(invs, new ItemData(i)));
    }

    private void TryAddItem(List<ComponentData> invs, ItemData itemData)
    {
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