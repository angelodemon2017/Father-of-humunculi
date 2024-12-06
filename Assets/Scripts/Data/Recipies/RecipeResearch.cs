using UnityEngine;

[CreateAssetMenu(menuName = "Recipies/Recipe research", order = 1)]
public class RecipeResearch : RecipeSO
{
    public ResearchSO research;
    public int CountUpgrade;

    public override string TitleRecipe => research.Name;
    public override UIIconModel IconModelResult => new UIIconModel(this);

    internal override bool AvailableRecipe(EntityData entityData)
    {
        var baseAvail = base.AvailableRecipe(entityData);

        var isDone = ResearchLibrary.Instance.IsResearchComplete(research.Name);

        return baseAvail && !isDone;
    }

    internal override void ReleaseRecipe(EntityData entityData, string args = "")
    {
        ResearchLibrary.Instance.Upgrade(this);
        entityData.UpdateEntity();
    }
}