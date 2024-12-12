using UnityEngine;
using static OptimazeExtensions;

public class ComponentUIlabels : ComponentData
{
    public Color TextColor;
    public string TextView = string.Empty;

    public ComponentUIlabels() : base(TypeCache<ComponentUIlabels>.IdType)
    {

    }
}