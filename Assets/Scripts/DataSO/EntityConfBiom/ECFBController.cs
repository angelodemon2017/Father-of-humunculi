using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ECFBController
{
    private static List<EntityConfigForBiom> _ecfbs = new();

    public static List<EntityConfigForBiom> GetECFBs()
    {
        Init();

        return _ecfbs;
    }

    public static EntityConfigForBiom GetECFB()
    {
        Init();

        return _ecfbs.FirstOrDefault();
    }

    private static void Init()
    {
        if (_ecfbs.Count == 0)
        {
            var tempItems = Resources.LoadAll<EntityConfigForBiom>(Config.PathECFBs).ToList();
            tempItems.ForEach(i => _ecfbs.Add(i));
        }
    }
}