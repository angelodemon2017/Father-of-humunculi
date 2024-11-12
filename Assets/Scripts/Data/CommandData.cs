public struct CommandData
{
    public long IdEntity;
    public string KeyComponent;
    public string KeyCommand;
    /// <summary>
    /// Arguments
    /// </summary>
    public string Message;

    public CommandData(long id, string component, string message = "")
    {
        IdEntity = id;
        KeyComponent = component;
        KeyCommand = string.Empty;
        Message = message;
    }
}