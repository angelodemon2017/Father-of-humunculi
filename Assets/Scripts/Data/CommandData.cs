public struct CommandData
{
    public long IdEntity;
    public int KeyComponent;
    public int AddingKeyComponent;
    public string KeyCommand;
    /// <summary>
    /// Arguments
    /// </summary>
    public string Message;

    public CommandData(long id, int component, string message = "")
    {
        IdEntity = id;
        KeyComponent = component;
        AddingKeyComponent = 0;
        KeyCommand = string.Empty;
        Message = message;
    }
}