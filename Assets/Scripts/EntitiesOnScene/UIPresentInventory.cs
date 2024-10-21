using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPresentInventory : MonoBehaviour
{
    [SerializeField] private GameObject _iconPrefab;
    [SerializeField] private Transform _parentIcons;

    private ComponentInventory _componentInventory;

    public void Init(ComponentInventory componentInventory)
    {
        _componentInventory = componentInventory;
        for (int i = 0; i < _componentInventory.MaxItems; i++)
        {

        }
    }
}