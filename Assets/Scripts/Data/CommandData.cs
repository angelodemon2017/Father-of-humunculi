public struct CommandData
{
    public long IdEntity;
    public string KeyComponent;
    public string AddingKeyComponent;
    public string KeyCommand;
    /// <summary>
    /// Arguments
    /// </summary>
    public string Message;

    public CommandData(long id, string component, string message = "")
    {
        IdEntity = id;
        KeyComponent = component;
        AddingKeyComponent = string.Empty;
        KeyCommand = string.Empty;
        Message = message;
    }
}