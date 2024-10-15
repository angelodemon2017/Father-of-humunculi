using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(menuName = "PrefabsByComponent", order = 1)]
public class PrefabsByComponent : ScriptableObject
{
    public static bool isInit = false;
    private const string _pathPrefabs = "PrefabsByComponentsData";
    [SerializeField] private List<PrefabByComponentData> _prefabs = new();

    public PrefabByComponentData GetPrefab(string keyComponent)
    {
        var result = _prefabs.FirstOrDefault(p => p.KeyComponent == keyComponent);

        if (!isInit)
        {
            _prefabs.Clear();
            var allgos = Resources.LoadAll<PrefabByComponentData>(_pathPrefabs).ToList();
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
    {
        EditorUtility.DisplayDialog("MyTool", "Do It in C# !", "OK", "");
    }
}