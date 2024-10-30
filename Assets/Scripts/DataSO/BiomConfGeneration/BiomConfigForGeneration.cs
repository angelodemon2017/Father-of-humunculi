using UnityEngine;

[CreateAssetMenu(menuName = "BCFG", order = 1)]
public class BiomConfigForGeneration : ScriptableObject
{
    public BiomSO _biomSO;
    public int _weightBiom;
    public TypeGeneration _typeGeneration;
}