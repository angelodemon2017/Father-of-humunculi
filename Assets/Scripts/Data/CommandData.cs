public struct CommandData
{
    public long IdEntity;
    public string KeyCommand;
    /// <summary>
    /// Arguments
    /// </summary>
    public string Message;

    public CommandData(long id, string component, string message = "")
    {
        IdEntity = id;
        KeyCommand = component;
        Message = message;
    }
}