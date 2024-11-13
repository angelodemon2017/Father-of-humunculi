public class ComponentUICraftGroup : ComponentData
{
    private string _recipeGroup;

    public string RecipeGroup => _recipeGroup;

    public ComponentUICraftGroup(string recipeGroup)
    {
        _recipeGroup = recipeGroup;
    }
}