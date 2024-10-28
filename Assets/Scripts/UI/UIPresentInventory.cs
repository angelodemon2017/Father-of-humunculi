using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPresentInventory : MonoBehaviour
{
    [SerializeField] private UIIconPresent _iconPrefab;
    [SerializeField] private Transform _parentIcons;
    [SerializeField] private UIPanelHint _uiPanelHint;

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

            newSlot.OnClickIcon += UseSlot;
            newSlot.OnClickRBM += ClickSlotRBM;
            newSlot.OnDragHandler += DragSlot;
            newSlot.OnDropHandler += DropSlot;
            newSlot.OnPointerEnter += PointerEnter;
            newSlot.OnPointerExit += PointerExit;

            _inventorySlots.Add(newSlot);
        }

        UpdateSlots();
    }

    private void DragSlot(int idSlot)
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
            hintModel.UseHints.Add("* ПКМ - разделение предмета на 2 слота");
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

    private void ClickSlotRBM(int idSlot)
    {
        _componentInventory.SplitSlot(idSlot);
        UpdateSlots();
    }

    private IEnumerator Crunch()
    {
        yield return new WaitForSeconds(0.1f);
        _parentIcons.gameObject.SetActive(false);
        _parentIcons.gameObject.SetActive(true);
    }

    public void UpdateSlots()
    {
        for (int i = 0; i < _inventorySlots.Count; i++)
        {
            var iconModel = new UIIconModel(i, _componentInventory.Items[i]);

            _inventorySlots[i].InitIcon(iconModel);
        }
    }

    private void OnDestroy()
    {
        foreach (var ci in _inventorySlots)
        {
            ci.OnClickIcon -= UseSlot;
            ci.OnDragHandler -= DragSlot;
            ci.OnDropHandler -= DropSlot;
            ci.OnClickRBM -= ClickSlotRBM;
        }
    }
}