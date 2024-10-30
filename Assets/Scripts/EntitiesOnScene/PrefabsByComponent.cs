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
            //            Debug.Log($"Loaded {_prefabs.Count} PrefabByComponentData");
            isInit = true;
        }

        return result;
    }
}