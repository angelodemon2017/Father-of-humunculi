using UnityEngine;
using System.Linq;
using System.Collections.Generic;
//using SimplexNoise;

[CreateAssetMenu(menuName = "Type Generation/Simplex Generation", order = 1)]
public class SimplexGeneration : TypeGeneration
{
    [Range(0, 1)]
    [SerializeField] private float SimpleScale;
    [Range(-1, 1)]
//    [SerializeField] private float HolesMap;
    [SerializeField] private BiomSO biom;

    public override WorldTileData GenerateTile(int x, int z, SeedData seed)
    {
        //        var mapNoise = int.Parse(seed.MapNoiseFrom10To99) * 100;

        var pr = Random.Range(0f, 1f);//Noise.CalcPixel2D(x, z, SimpleScale);
        //        Mathf.PerlinNoise((x - mapNoise) * PerlinScale, (z + mapNoise) * PerlinScale);

        var index = pr * biom._textures.Count;// + HolesMap;
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