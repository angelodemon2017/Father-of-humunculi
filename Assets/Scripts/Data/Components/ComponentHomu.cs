using System.Collections.Generic;
using UnityEngine;

public class ComponentHomu : ComponentData
{
    private Dictionary<EnumHomuType, Color> colors = new()
    {
        { EnumHomuType.None, Color.white },
        { EnumHomuType.Dummy, Color.white },
        { EnumHomuType.Stone, Color.gray },
        { EnumHomuType.Wood, Color.yellow },
        { EnumHomuType.Leaf, Color.green },
    };

    public long IdInFocus = -1;
    public HashSet<long> _idsInFocus = new();
    public EnumHomuRole HomuRole;
    public int _thinkingTime = 0;

    private EnumHomuType _homuType = EnumHomuType.Dummy;
    public Color _colorModelDemo => colors[_homuType];
    public string _titleDemo => $"Homu {_homuType}";
    public bool IsNoType => _homuType == EnumHomuType.None || _homuType == EnumHomuType.Dummy;

    public ComponentHomu()
    {

    }

    public ComponentHomu(EnumHomuType homuType)
    {
        SetHomuType(homuType);
    }

    public void ApplyRecipe(RecipeSO recipe)
    {
        SetHomuType(recipe.homuType);
    }

    private void SetHomuType(EnumHomuType homuType)
    {
        _homuType = homuType;
    }

    public void AddIdFocus(long id)
    {
        _idsInFocus.Add(id);
    }
}