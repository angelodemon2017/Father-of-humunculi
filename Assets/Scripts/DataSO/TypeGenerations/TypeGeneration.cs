using UnityEngine;

[CreateAssetMenu(menuName = "Type Generation/Zero texture generation", order = 1)]
public class TypeGeneration : ScriptableObject
{
    [HideInInspector]
    public int index;
    public int _TypeGeneration;

    public virtual WorldTileData GenerateTile(int x, int z, SeedData seed)
    {
        var txt = WorldViewer.Instance.Textures[1];
        return new WorldTileData(txt.Id, x, z);
    }
}