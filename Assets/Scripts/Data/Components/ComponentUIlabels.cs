using UnityEngine;

public class ComponentUIlabels : ComponentData
{
    public Color TextColor;
    public float High;
    public string TextView = string.Empty;

    public ComponentUIlabels(Color setColor, float high = 1f)
    {
        TextColor = setColor;
        High = high;
    }

    internal override void UpdateAfterEntityUpdate(EntityData entity)
    {
        var cmp = entity.Components.GetComponent<ComponentCounter>();
        if (cmp != null)
        {
            TextView = $"res: {cmp._debugCounter}";
        }
    }
    //bundle data for ui generate
}