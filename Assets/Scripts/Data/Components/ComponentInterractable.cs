using System;

public class ComponentInterractable : ComponentData
{
    public string TipKey => _gettingTip?.Invoke();

    private Func<string> _gettingTip;

    public ComponentInterractable()
    {

    }

    public ComponentInterractable(Func<string> gettingTip)
    {
        _gettingTip = gettingTip;
    }

    public static CommandData GetTouch(string idWhoTouched)
    {
        return new CommandData(-1, typeof(ComponentInterractable).Name, idWhoTouched);
    }
}