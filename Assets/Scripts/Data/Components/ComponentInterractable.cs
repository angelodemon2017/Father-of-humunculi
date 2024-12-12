using System;
using static OptimazeExtensions;

public class ComponentInterractable : ComponentData
{
    public string TipKey => _gettingTip?.Invoke();

    private Func<string> _gettingTip;

    public ComponentInterractable() : base(TypeCache<ComponentInterractable>.IdType)
    {

    }

    public static CommandData GetTouch(string idWhoTouched)
    {
        return new CommandData(-1, TypeCache<ComponentInterractable>.IdType/*??*/, idWhoTouched);
    }
}