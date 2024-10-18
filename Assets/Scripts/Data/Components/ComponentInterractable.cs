public class ComponentInterractable : ComponentData
{
    public string TipKey;

    public ComponentInterractable(string tip)
    {
        TipKey = tip;
    }

    public static CommandData GetTouch(string idWhoTouched)
    {
        return new CommandData(-1, typeof(ComponentInterractable).Name, idWhoTouched);
    }
}