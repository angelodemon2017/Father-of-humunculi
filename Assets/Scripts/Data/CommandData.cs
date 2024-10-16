public struct CommandData
{
    public long IdEntity;
    /// <summary>
    /// Arguments
    /// </summary>
    public string Message;

    public CommandData(long id, string message = "")
    {
        IdEntity = id;
        Message = message;
    }
}