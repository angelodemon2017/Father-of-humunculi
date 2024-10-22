using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class RecipesController
{
    private static List<RecipeSO> _recipes = new();

    public static List<RecipeSO> GetRecipes(string groupRecipe)
    {
        if (_recipes.Count == 0)
        {
            var tempRecipes = Resources.LoadAll<RecipeSO>(Config.PathRecipes).ToList();
            tempRecipes.ForEach(i => _recipes.Add(i));
        }

        return _recipes.Where(r => r.GroupRecipeTag.GroupName == groupRecipe).ToList();
    }
}