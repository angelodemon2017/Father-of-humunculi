using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BiomsController
{
    private static List<BiomSO> _bioms = new();

    public static List<BiomSO> GetBioms()
    {
        Init();

        return _bioms;
    }

    public static BiomSO GetBiom()
    {
        Init();

        return _bioms.FirstOrDefault();
    }

    private static void Init()
    {
        if (_bioms.Count == 0)
        {
            var tempItems = Resources.LoadAll<BiomSO>(Config.PathBioms).ToList();
            tempItems.ForEach(i => _bioms.Add(i));
        }
    }
}