public class ComponentUICraftGroup : ComponentData
{
    private long _whoOpened;
    private string _recipeGroup;

    public long WhoOpened => _whoOpened;
    public string RecipeGroup => _recipeGroup;

    public ComponentUICraftGroup(string recipeGroup)
    {
        _recipeGroup = recipeGroup;
    }

    public void SetEntityOpener(long idEntity)
    {
        _whoOpened = idEntity;
    }
}