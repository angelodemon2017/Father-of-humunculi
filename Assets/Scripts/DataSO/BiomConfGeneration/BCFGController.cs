using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BCFGController
{
    private static List<BiomConfigForGeneration> _bcfgs = new();

    public static List<BiomConfigForGeneration> GetBioms(int typeGeneration)
    {
        Init();

        return _bcfgs.Where(b => b._typeGeneration._TypeGeneration == typeGeneration).ToList();
    }

    private static void Init()
    {
        if (_bcfgs.Count == 0)
        {
            var tempItems = Resources.LoadAll<BiomConfigForGeneration>(Config.PathBCFGs).ToList();
            tempItems.ForEach(i => _bcfgs.Add(i));
        }
    }
}