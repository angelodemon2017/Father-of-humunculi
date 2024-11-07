using UnityEngine;

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

    /// <summary>
    /// SET BUILD
    /// </summary>
    public CommandData(EntityData entityData, RecipeSO recipe, Vector3 position)
    {
        IdEntity = entityData.Id;
        KeyCommand = Dict.Commands.SetBuild;
        Message = $"{recipe.Index}^{position.x}^{position.z}";
    }
}