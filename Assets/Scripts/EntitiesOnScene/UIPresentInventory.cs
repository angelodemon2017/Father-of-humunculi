using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPresentInventory : MonoBehaviour
{
    [SerializeField] private UIIconInventory _iconPrefab;
    [SerializeField] private Transform _parentIcons;

    private ComponentInventory _componentInventory;
    private List<UIIconInventory> _inventorySlots = new();

    public Action ComponentUpdated;

    public void Init(ComponentInventory componentInventory)
    {
        _componentInventory = componentInventory;
        _parentIcons.DestroyChildrens();
        for (int i = 0; i < _componentInventory.MaxItems; i++)
        {
            var newSlot = Instantiate(_iconPrefab, _parentIcons);
            newSlot.OnClickItem += ClickItem;
            //may be subscribe on item
            _inventorySlots.Add(newSlot);
        }

        StartCoroutine(Crunch());
        UpdateSlots();
    }

    private void ClickItem(ItemData item)//TODO needCommand
    {
        _componentInventory.DropItem(item);
        ComponentUpdated?.Invoke();
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
            if (_componentInventory.Items.Count > i)
            {
                _inventorySlots[i].Init(_componentInventory.Items[i]);
            }
            else
            {
                _inventorySlots[i].InitEmpty();
            }
        }
    }

    private void OnDestroy()
    {
        foreach (var ci in _inventorySlots)
        {
            ci.OnClickItem -= ClickItem;
        }
    }
}