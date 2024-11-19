using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEquipmentView : MonoBehaviour
{
    [SerializeField] private UIIconPresent _iconPrefab;
    [SerializeField] private Transform _parentIcons;
    [SerializeField] private BaseInventoryAdapter _baseInventoryAdapter;

    private ComponentInventory _componentInventory;
    private List<UIIconPresent> _inventorySlots = new();

    public void Init(ComponentInventory componentInventory, EntityInProcess entityInProcess)
    {
        _componentInventory = componentInventory;
        _parentIcons.DestroyChildrens();

        for (int i = 0; i < _componentInventory.MaxItems; i++)
        {
            var newSlot = Instantiate(_iconPrefab, _parentIcons);

            _inventorySlots.Add(newSlot);
        }

        _baseInventoryAdapter.Init(_componentInventory, entityInProcess);
        _baseInventoryAdapter.InitSlots(_inventorySlots);
    }
}