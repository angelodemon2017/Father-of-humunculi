using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class GroupController
{
    private static List<GroupSO> _recipes = new();

    public static List<GroupSO> GetAllGroups()
    {
        if (_recipes.Count == 0)
        {
            var tempRecipes = Resources.LoadAll<GroupSO>(Config.PathRecipeGroups).ToList();
            tempRecipes.ForEach(i => _recipes.Add(i));
        }

        return _recipes;
    }
}