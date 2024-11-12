using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomuPresentPBCD : PrefabByComponentData
{
    [SerializeField] private SpriteRenderer _spriteRenderer;

    private ComponentHomu _component;

    public override string KeyComponent => typeof(HomuPresentPBCD).Name;
    public override string KeyComponentData => typeof(ComponentHomu).Name;
    internal override ComponentData GetComponentData => GetCompHomu();
    public override string GetDebugText => _component._titleDemo;

    public override void Init(ComponentData componentData, EntityInProcess entityInProcess = null)
    {
        _component = (ComponentHomu)componentData;

        UpdateHome();
    }

    private ComponentHomu GetCompHomu()
    {
        return new ComponentHomu();
    }

    private void UpdateHome()
    {
        _spriteRenderer.color = _component._colorModelDemo;
    }
}