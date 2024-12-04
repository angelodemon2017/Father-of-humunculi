using UnityEngine;

[CreateAssetMenu(menuName = "Recipies/Recipe homu", order = 1)]
public class RecipeHomu : RecipeSO
{
    public EnumHomuType HomuType;

    public override string TitleRecipe => $"{HomuType}";

    internal override void ReleaseRecipe(EntityData entityData, string args = "")
    {
        var cmpHomu = entityData.Components.GetComponent<ComponentHomu>();
        cmpHomu.SetHomuType(HomuType);
    }
}