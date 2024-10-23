using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPresentInventory : MonoBehaviour
{
    [SerializeField] private UIIconPresent _iconPrefab;
    [SerializeField] private Transform _parentIcons;

    private ComponentInventory _componentInventory;
    private List<UIIconPresent> _inventorySlots = new();

    public Action ComponentUpdated;



    /// <summary>
    /// TEST!!!
    /// </summary>
    public void ApplyRecipe(RecipeSO recipe)
    {
        _componentInventory.TryApplyRecipe(recipe);
        ComponentUpdated?.Invoke();
    }

    public void Init(ComponentInventory componentInventory)
    {
        _componentInventory = componentInventory;
        _parentIcons.DestroyChildrens();
        for (int i = 0; i < _componentInventory.MaxItems; i++)
        {
            var newSlot = Instantiate(_iconPrefab, _parentIcons);
            newSlot.OnClickIcon += ClickSlot;
            _inventorySlots.Add(newSlot);
        }

        StartCoroutine(Crunch());
        UpdateSlots();
    }

    private void ClickSlot(int idButton)
    {
        _componentInventory.DropSlot(idButton);
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
            var iconModel = new UIIconModel(i, _componentInventory.Items[i]);

            _inventorySlots[i].InitIcon(iconModel);
        }
    }

    private void OnDestroy()
    {
        foreach (var ci in _inventorySlots)
        {
            ci.OnClickIcon -= ClickSlot;
        }
    }
}