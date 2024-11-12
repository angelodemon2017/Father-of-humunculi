using UnityEngine;

public class InventoryPBCD : PrefabByComponentData
{
    private const char splitter = '^';

    [SerializeField] private EntityMonobeh _dropItem;
    [SerializeField] private int _maxItems = 5;
    private ComponentInventory _component;

    public override string KeyComponent => typeof(InventoryPBCD).Name;
    public override string KeyComponentData => typeof(ComponentInventory).Name;

    internal override ComponentData GetComponentData => new ComponentInventory(_maxItems);

    public override void Init(ComponentData componentData, EntityInProcess entityInProcess = null)
    {
        _component = (ComponentInventory)componentData;
    }

    public override void ExecuteCommand(EntityData entity, string command, string message, WorldData worldData)
    {
        switch (command)
        {
            case Dict.Commands.SetEntity:
                SetEntityByRecipe(entity, message, worldData);
                break;
            default:
                break;
        }
    }

    private void SetEntityByRecipe(EntityData entity, string message, WorldData worldData)
    {
        var compInv = entity.Components.GetComponent<ComponentInventory>();
        if (compInv != null)
        {
            var mess = message.Split(splitter);
            var recipe = RecipesController.GetRecipe(int.Parse(mess[0]));

            if (compInv.AvailableRecipe(recipe))
            {
                compInv.SubtrackItemsByRecipe(recipe);

                var newEntity = recipe._entityConfig.CreateEntity(float.Parse(mess[1]), float.Parse(mess[2]));
                var compHomu = newEntity.Components.GetComponent<ComponentHomu>();
                if (compHomu != null)
                {
                    compHomu.ApplyRecipe(recipe);
                }
                worldData.AddEntity(newEntity);
            }
        }
    }

    public CommandData GetCommandSetEntity(EntityData entityData, RecipeSO recipe, Vector3 position)
    {
        return new CommandData()
        {
            IdEntity = entityData.Id,
            KeyComponent = KeyComponent,
            KeyCommand = Dict.Commands.SetEntity,
            Message = $"{recipe.Index}{splitter}{position.x}{splitter}{position.z}",
        };
    }
}