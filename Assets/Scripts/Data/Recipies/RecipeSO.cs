using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Recipies/Recipe orig", order = 1)]
public class RecipeSO : ScriptableObject
{
    [HideInInspector]
    public int Index;

//    public ElementRecipe Result;

    public List<ElementRecipe> Resources = new();

    public GroupSO GroupRecipeTag;

    public virtual UIIconModel IconModelResult => null;// new UIIconModel();
    public virtual string TitleRecipe => string.Empty;// Result.ItemConfig.Key;

    internal virtual void ReleaseRecipe(EntityData entityData, string args = "")
    {

    }
}