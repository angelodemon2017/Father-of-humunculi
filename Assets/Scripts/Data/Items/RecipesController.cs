using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class RecipesController
{
    private static Dictionary<int, RecipeSO> _recipes = new();

    public static List<RecipeSO> GetRecipes(string groupRecipe)
    {
        if (_recipes.Count == 0)
        {
            var tempRecipes = Resources.LoadAll<RecipeSO>(Config.PathRecipes).ToList();
            for (int i = 0; i < tempRecipes.Count; i++)
            {
                _recipes.Add(i, tempRecipes[i]);
            }
        }
        //this was crunch, need rewrite
        return _recipes.Where(r => r.Value.GroupRecipeTag.GroupName == groupRecipe).Select(r => r.Value).ToList();
    }
}