using UnityEngine;

[CreateAssetMenu(menuName = "Recipies/Recipe research", order = 1)]
public class RecipeResearch : RecipeSO
{
    public ResearchSO research;
    public int CountUpgrade;

    public override string TitleRecipe => research.Name;
    public override UIIconModel IconModelResult => new UIIconModel(research);

    internal override void ReleaseRecipe(EntityData entityData, string args = "")
    {
        ResearchLibrary.Instance.Upgrade(this);
        entityData.UpdateEntity();
    }
}