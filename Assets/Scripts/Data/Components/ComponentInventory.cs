using System.Collections.Generic;
using UnityEngine;

public class ComponentInventory : ComponentData
{
    public int SomeResource;
    public List<ItemData> Items;

    public override void Init(Transform entityME)
    {

    }

    public void AddSource(int newSource)
    {
        SomeResource += newSource;
    }
}