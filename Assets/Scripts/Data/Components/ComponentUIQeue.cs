public class ComponentUIQeue : ComponentData
{
    private long _whoOpened;
    private string _recipeGroup;
    private int _craftProcess;
    private string _selectRecipe;

    public long WhoOpened => _whoOpened;
    public string RecipeGroup => _recipeGroup;

    public ComponentUIQeue(string recipeGroup)
    {
        _recipeGroup = recipeGroup;
    }

    public void SetEntityOpener(long idEntity)
    {
        _whoOpened = idEntity;
    }
}