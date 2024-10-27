using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class GroupController
{
    private static List<GroupSO> _groups = new();

    public static List<GroupSO> GetAllGroups()
    {
        Init();

        return _groups;
    }

    public static GroupSO GetGroup(string groupName)
    {
        Init();

        return _groups.FirstOrDefault(g => g.GroupName == groupName);
    }

    private static void Init()
    {
        if (_groups.Count == 0)
        {
            var tempRecipes = Resources.LoadAll<GroupSO>(Config.PathRecipeGroups).ToList();
            tempRecipes.ForEach(i => _groups.Add(i));
        }
    }
}