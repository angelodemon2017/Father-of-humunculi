using UnityEngine;

[CreateAssetMenu(menuName = "ECFB", order = 1)]
public class EntityConfigForBiom : ScriptableObject
{
    public EntitySO _entitySO;
    public int _weightEntity;
    public BiomSO _biomSO;
}