using UnityEngine;

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

    /// <summary>
    /// SET BUILD
    /// </summary>
    public CommandData(EntityData entityData, RecipeSO recipe, Vector3 position)
    {
        IdEntity = entityData.Id;
        Component = Dict.Commands.SetBuild;
        Message = $"{recipe.Index}^{position.x}^{position.z}";
    }
}