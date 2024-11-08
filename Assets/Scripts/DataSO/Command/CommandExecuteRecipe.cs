using UnityEngine;

[CreateAssetMenu(menuName = "Command/Command Execute Recipe", order = 1)]
public class CommandExecuteRecipe : CommandExecuterSO
{
    private const char splitter = '^';

    internal override string Key => Dict.Commands.SetBuild;//???

    public override void Execute(EntityData entity, string message, WorldData worldData)
    {
        var compInv = entity.Components.GetComponent<ComponentInventory>();
        if (compInv != null)
        {
            var mess = message.Split(splitter);
            var recipe = RecipesController.GetRecipe(int.Parse(mess[0]));

            if (compInv.AvailableRecipe(recipe))
            {
                compInv.SubtrackItemsByRecipe(recipe);

                var newEntity = recipe._entitySOBuild.CreateEntity(float.Parse(mess[1]), float.Parse(mess[2]));
                GameProcess.Instance.GameWorld.AddEntity(newEntity);
            }
        }
    }

    public static CommandData GetCommand(EntityData entityData, RecipeSO recipe, Vector3 position)
    {
        return new CommandData()
        {
            IdEntity = entityData.Id,
            KeyCommand = Dict.Commands.SetBuild,
            Message = $"{recipe.Index}{splitter}{position.x}{splitter}{position.z}",
        };
    }
}