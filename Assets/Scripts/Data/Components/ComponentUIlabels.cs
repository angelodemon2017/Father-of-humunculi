using UnityEngine;

public class ComponentUIlabels : ComponentData
{
    public Color TextColor;
    public float High;

    public ComponentUIlabels(Color setColor, float high = 1f)
    {
        TextColor = setColor;
        High = high;
    }
    //bundle data for ui generate
}