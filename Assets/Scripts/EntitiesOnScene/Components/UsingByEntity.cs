using System.Collections.Generic;
using UnityEngine;

public class UsingByEntity : PrefabByComponentData
{
    [SerializeField] private List<GameObject> _onOffsObjects;

    public override string KeyComponent => typeof(UsingByEntity).Name;
    public override string KeyComponentData => typeof(ComponentUsingByEntity).Name;

    internal override ComponentData GetComponentData => new ComponentUsingByEntity();

    public override void Init(ComponentData componentData, EntityInProcess entityInProcess = null)
    {
        var _component = (ComponentUsingByEntity)componentData;
    }
}