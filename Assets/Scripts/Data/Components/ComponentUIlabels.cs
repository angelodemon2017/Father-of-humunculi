using UnityEngine;

public class ComponentUIlabels : ComponentData
{
    public Color TextColor;
    public float High;
    public string TextView = string.Empty;

    public ComponentUIlabels()
    {

    }

    public ComponentUIlabels(Color setColor, float high = 1f, string defaulthText = "")
    {
        TextColor = setColor;
        High = high;
        TextView = defaulthText;
    }

/*    internal override void UpdateAfterEntityUpdate(EntityData entity)
    {
        var cmp = entity.Components.GetComponent<ComponentCounter>();
        if (cmp != null)
        {
            TextView = $"res: {cmp._debugCounter}";
        }

        var cmpHom = entity.Components.GetComponent<ComponentHomu>();
        if (cmpHom != null)
        {
            TextView = cmpHom._titleDemo;
        }
    }/**/
    //bundle data for ui generate
}