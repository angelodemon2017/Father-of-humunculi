using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.EntitiesOnScene
{
    [CreateAssetMenu(menuName = "PrefabsByComponent", order = 1)]
    public class PrefabsByComponent : ScriptableObject
    {
        [SerializeField] private List<PrefabByComponentData> _prefabs = new();

        [MenuItem("Tools/MyTool/Do It in C#")]
        static void DoIt()
        {
            EditorUtility.DisplayDialog("MyTool", "Do It in C# !", "OK", "");
        }
    }
}