using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu(menuName = "Type Generation/Multi Layer Perlin Generation", order = 1)]
public class MultiLayersPerlinGeneration : TypeGeneration
{
    [Range(0, 1)]
    [SerializeField] private float PerlinHighScale;
    [Range(0, 1)]
    [SerializeField] private float PerlinToxicScale;

    [SerializeField] private List<LayerHigh> _layersHigh = new();

    public override WorldTileData GenerateTile(int x, int z, SeedData seed)
    {
        var layer = GetToxicLayer(x, z, seed);

        return new WorldTileData(layer.MainTexture.Id, x, z, layer.Biom.Key);
    }

    public EntityMonobeh GenEntity(int x, int z, SeedData seed)
    {
        var lh = GetLayerHigh(x, z, seed);

        var pr2 = Perlin(x, z, seed.MapLayer2, PerlinToxicScale);

        return lh.GetByPerlin(pr2).GenEntity(pr2);
    }

    private LayerHigh GetLayerHigh(int x, int z, SeedData seed)
    {
        var pr1 = Perlin(x,z, seed.MapNoise, PerlinHighScale);

        return _layersHigh.FirstOrDefault(l => l.IsInside(pr1));
    }

    private LayerToxic GetToxicLayer(int x, int z, SeedData seed)
    {
        var lh = GetLayerHigh(x, z, seed);

        var pr2 = Perlin(x, z, seed.MapLayer2, PerlinToxicScale);

        return lh.GetByPerlin(pr2);
    }

    private float Perlin(int x, int z, int seed, float scale)
    {
        var mapNoise1 = seed * 100;

        return Mathf.PerlinNoise((x - mapNoise1) * scale, (z + mapNoise1) * scale).FixMinMax(0, 1);
    }

    [System.Serializable]
    internal class LayerHigh
    {
        public string Name;
        [Range(0, 1)]
        public float MinVal = 0;
        [Range(0, 1)]
        public float MaxVal = 1;

        public List<LayerToxic> layerToxics = new();

        public bool IsInside(float pr)
        {
            return pr >= MinVal && pr <= MaxVal;
        }

        public LayerToxic GetByPerlin(float toxic)
        {
            return layerToxics.FirstOrDefault(l => l.IsInside(toxic));
        }
    }

    [System.Serializable]
    internal class LayerToxic
    {
        [Range(0, 1)]
        public float MinVal = 0;
        [Range(0, 1)]
        public float MaxVal = 1;

        public TextureEntity MainTexture;
        public BiomByLayerPerlinGeneration Biom;

        public bool IsInside(float pr)
        {
            return pr >= MinVal && pr <= MaxVal;
        }

        public EntityMonobeh GenEntity(float pr)
        {
            var dif = pr - MinVal / MaxVal - MinVal;

            return Biom.GetRndEntity(dif);
        }
    }
}