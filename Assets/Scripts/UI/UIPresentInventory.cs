using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPresentInventory : MonoBehaviour
{
    [SerializeField] private UIIconPresent _iconPrefab;
    [SerializeField] private Transform _parentIcons;
    [SerializeField] private UIPanelHint _uiPanelHint;
    [SerializeField] private BaseInventoryAdapter _baseInventoryAdapter;

    private ComponentInventory _componentInventory;
    private List<UIIconPresent> _inventorySlots = new();

    public Action OnComponentUpdated;
    public Action<ItemData> OnDragItem;
    public Action<ItemData> OnDropItem;
    public Action<int> OnUseItem;

    public void Init(ComponentInventory componentInventory)
    {
        _uiPanelHint.Hide();

        _componentInventory = componentInventory;
        _parentIcons.DestroyChildrens();
        for (int i = 0; i < _componentInventory.MaxItems; i++)
        {
            var newSlot = Instantiate(_iconPrefab, _parentIcons);

/*            newSlot.OnClickIcon += UseSlot;
            newSlot.OnClickMBM += ClickSlotMBM;
            newSlot.OnDragHandler += DragSlot;
            newSlot.OnDropHandler += DropSlot;
            newSlot.OnPointerEnter += PointerEnter;
            newSlot.OnPointerExit += PointerExit;/**/

            _inventorySlots.Add(newSlot);
        }

//        UpdateSlots();
        _baseInventoryAdapter.InitSlots(_inventorySlots);
    }

/*    private void DragSlot(int idSlot)
    {
        var item = _componentInventory.Items[idSlot];
        if (item.IsEmpty)
        {
            return;
        }

        OnDragItem?.Invoke(item);
    }

    private void DropSlot(int idSlot)
    {
        var item = _componentInventory.Items[idSlot];

        OnDropItem?.Invoke(item);
    }

    private void PointerEnter(int index)
    {
        var item = _componentInventory.Items[index];
        if (item.IsEmpty)
        {
            return;
        }

        var hintModel = new HintModel()
        {
            Icon = item.ItemConfig.IconItem,
            Title = item.ItemConfig.Key,
            Description = item.ItemConfig.Description,
        };
        if (item.Count > 1)
        {
            hintModel.UseHints.Add("* СКМ - разделение предмета на 2 слота");
        }
        if (item.ItemConfig.ItemActions.Count > 0)
        {
            hintModel.UseHints.Add("* ЛКМ - использование предмета");
        }

        _uiPanelHint.Init(hintModel);
        _uiPanelHint.transform.position = _inventorySlots[index].transform.position;
    }

    private void PointerExit(int index)
    {
        _uiPanelHint.Hide();
    }

    private void UseSlot(int idButton)
    {
        OnUseItem?.Invoke(idButton);
    }

    private void ClickSlotMBM(int idSlot)
    {
        _componentInventory.SplitSlot(idSlot);
        UpdateSlots();
    }

    public void UpdateSlots()
    {
        for (int i = 0; i < _inventorySlots.Count; i++)
        {
            var iconModel = new UIIconModel(i, _componentInventory.Items[i]);

            _inventorySlots[i].InitIcon(iconModel);
        }
    }/**/

    private void OnDestroy()
    {
        foreach (var ci in _inventorySlots)
        {
/*            ci.OnClickIcon -= UseSlot;
            ci.OnDragHandler -= DragSlot;
            ci.OnDropHandler -= DropSlot;
            ci.OnClickMBM -= ClickSlotMBM;/**/
        }
    }
}