using System;
using UnityEngine;

public class ComponentModelByRectPrefab : ComponentData
{
    private string _itemKey;

    public ComponentModelByRectPrefab()
    {

    }

    public ComponentModelByRectPrefab(string itemKey)
    {
        _itemKey = itemKey;
    }

    //TODO BIG QUEST IS NEED COMPONENT
    //    public string KeyModel;
    /*    public Sprite _spriteRenderer;

        public ComponentModelByRectPrefab(Sprite sprite)
        {
            _spriteRenderer = sprite;
        }/**/

    /*    public ComponentModelByRectPrefab(string keyModel) : base()
        {
    //        KeyModel = keyModel;
        }/**/
}