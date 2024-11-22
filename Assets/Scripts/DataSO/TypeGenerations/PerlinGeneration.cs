using UnityEngine;
using System.Linq;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Type Generation/Perlin Generation", order = 1)]
public class PerlinGeneration : TypeGeneration
{
    [Range(0, 1)]
    [SerializeField] private float PerlinScale;
    [Range(-1, 1)]
    [SerializeField] private float HolesMap;
    [SerializeField] private BiomSO biom;

    public override WorldTileData GenerateTile(int x, int z, SeedData seed)
    {
        var mapNoise = int.Parse(seed.MapNoiseFrom10To99) * 100;

        var pr = Mathf.PerlinNoise((x - mapNoise) * PerlinScale, (z + mapNoise) * PerlinScale);

        var index = pr * biom._textures.Count + HolesMap;
        index = index.FixIndex(biom._textures.Count);
        var textur = biom._textures[(int)index];

        return new WorldTileData(textur.Id, x, z, biom.Key);
    }

    public override List<EntityData> GenerateEntitiesByChunk(List<WorldTileData> chunk, SeedData seed)
    {
        List<EntityData> result = new();



        return result;
    }
}