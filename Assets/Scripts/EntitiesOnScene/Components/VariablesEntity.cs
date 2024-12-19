using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VariablesEntity : PrefabByComponentData
{
    [SerializeField] private List<Sprite> _variables;
    [SerializeField] private RootSpriteRender _rootSpriteRender;

    private void Awake()
    {
        SelectVariant();
    }

    private void SelectVariant()
    {
        if (_variables.Count == 0)
        {
            return;
        }

        var tempSelect = _variables.GetRandom();
        _rootSpriteRender.SetSprite(tempSelect);
    }
}