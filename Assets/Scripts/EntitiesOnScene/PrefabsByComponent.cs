using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(menuName = "PrefabsByComponent", order = 1)]
public class PrefabsByComponent : ScriptableObject
{
    public static bool isInit = false;
    [SerializeField] private List<PrefabByComponentData> _prefabs = new();

    public PrefabByComponentData GetPrefab(string keyComponent)
    {
        var result = _prefabs.FirstOrDefault(p => p.KeyComponent == keyComponent);

        if (!isInit)
        {
            _prefabs.Clear();
            var allgos = Resources.LoadAll<PrefabByComponentData>(Config.PathPrefabsByComponents).ToList();
            foreach (var ass in allgos)
            {
                _prefabs.Add(ass);
            }

            result = _prefabs.FirstOrDefault(p => p.KeyComponent == keyComponent);
            Debug.Log($"Loaded {_prefabs.Count} PrefabByComponentData");
            isInit = true;
        }

        return result;
    }

    [MenuItem("Tools/MyTool/Do It in C#")]
    static void DoIt()
    {//better after 100 days
        double result = 0;
        for (int i = 0; i < 300; i++)
        {
            if (result > 100)
            {
                Debug.Log($"result :{result}, day{i}");

                break;
            }
            result += result / 100 + 1;
        }

        Debug.Log($"result :{result}");
//        EditorUtility.DisplayDialog("MyTool", "Do It in C# !", "OK", "");
    }
}