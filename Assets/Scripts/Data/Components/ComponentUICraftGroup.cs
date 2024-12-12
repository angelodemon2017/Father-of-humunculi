using static OptimazeExtensions;

public class ComponentUICraftGroup : ComponentData
{
    private string _recipeGroup;

    public string RecipeGroup => _recipeGroup;

    public ComponentUICraftGroup(string recipeGroup) : base(TypeCache<ComponentUICraftGroup>.IdType)
    {
        _recipeGroup = recipeGroup;
    }
}