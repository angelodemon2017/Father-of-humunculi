using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(menuName = "PrefabsByComponent", order = 1)]
public class PrefabsByComponent : ScriptableObject
{
    [SerializeField] private List<PrefabByComponentData<ComponentData>> _prefabs = new();

    public PrefabByComponentData<ComponentData> GetPrefab(string keyCoponent)
    {
        PrefabByComponentData<ComponentData> result = _prefabs.FirstOrDefault(p => p.KeyComponent == keyCoponent);

        return result;
    }

    [MenuItem("Tools/MyTool/Do It in C#")]
    static void DoIt()
    {
        EditorUtility.DisplayDialog("MyTool", "Do It in C# !", "OK", "");
    }
}