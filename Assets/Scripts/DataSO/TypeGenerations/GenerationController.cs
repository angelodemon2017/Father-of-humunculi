using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GenerationController : ScriptableObject
{
    private static List<TypeGeneration> _generations = new();

    public static List<TypeGeneration> GetTypeGenerations()
    {
        Init();

        return _generations;
    }

    public static TypeGeneration GetGeneration(int type)
    {
        Init();

        return _generations.FirstOrDefault(g => g.index == type);
    }

    private static void Init()
    {
        if (_generations.Count == 0)
        {
            var tempItems = Resources.LoadAll<TypeGeneration>(Config.PathTypeGenerations).ToList();

            for (var g = 0; g < tempItems.Count; g++)
            {
                tempItems[g].index = g;
                _generations.Add(tempItems[g]);
            }
        }
    }
}