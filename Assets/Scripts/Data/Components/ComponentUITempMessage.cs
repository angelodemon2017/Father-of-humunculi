public class ComponentUITempMessage : ComponentData
{
    public string TextView = string.Empty;
    public int SecondVision = 0;

    public bool IsShowMessage => SecondVision > 0;

    public ComponentUITempMessage(string defMessage)
    {
        TextView = defMessage;
    }
}