public struct CommandData
{
    public long IdEntity;
    public string Component;
    /// <summary>
    /// Arguments
    /// </summary>
    public string Message;

    public CommandData(long id, string component, string message = "")
    {
        IdEntity = id;
        Component = component;
        Message = message;
    }
}