using System.Collections.Generic;
using UnityEngine;

public class ComponentHomu : ComponentData
{
    private Dictionary<EnumHomuType, Color> colors = new()
    {
        { EnumHomuType.Stone, Color.gray },
        { EnumHomuType.Wood, Color.yellow },
        { EnumHomuType.Leaf, Color.green },
    };

    private EnumHomuType _homuType;
    public Color _colorModelDemo => colors[_homuType];
    public string _titleDemo => $"Homu {_homuType}";

    public ComponentHomu()
    {

    }

    public ComponentHomu(EnumHomuType homuType)
    {
        _homuType = homuType;
    }

    public void ApplyRecipe(RecipeSO recipe)
    {
        _homuType = recipe.homuType;
    }
}