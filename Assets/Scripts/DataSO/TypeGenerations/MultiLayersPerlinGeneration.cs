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
    [SerializeField] private bool EnableEntityGenerate;
    [SerializeField] private List<LayerHigh> _layersHigh = new();
    [SerializeField] private int _entitiesByChunk;

    public override WorldTileData GenerateTile(int x, int z, SeedData seed)
    {
        var layer = GetToxicLayer(x, z, seed);

        var pr1 = Perlin(x, z, seed.MapNoise, PerlinHighScale);
        var pr2 = Perlin(x, z, seed.MapLayer2, PerlinToxicScale);

        var lh = _layersHigh.FirstOrDefault(l => l.IsInside(pr1));
        var lt = lh.GetByPerlin(pr2);

//        var dif1 = lh.GetFif(pr1);
//        var dif2 = lt.GetDif(pr2);

//        var totalDif = (pr1 + pr2 - lh.MinVal + lt.MinVal) / (lh.MaxVal + lt.MaxVal - lh.MinVal + lt.MinVal);
//        string debug = $"______\r\n(h:{pr1.ToString("#.##")})\r\n(T:{pr2.ToString("#.##")})\r\n(R1:{dif1.ToString("#.##")})\r\n(R2:{dif2.ToString("#.##")})";

        bool isBorder = false;
        if (layer.BorderTexture != null)
            if (GetToxicLayer(x + 1, z, seed).Biom.Key != layer.Biom.Key ||
               GetToxicLayer(x - 1, z, seed).Biom.Key != layer.Biom.Key ||
               GetToxicLayer(x, z + 1, seed).Biom.Key != layer.Biom.Key ||
               GetToxicLayer(x, z - 1, seed).Biom.Key != layer.Biom.Key)
            {
                isBorder = true;
            }

        return new WorldTileData(isBorder ? layer.BorderTexture.Id : layer.MainTexture.Id, x, z, layer.Biom.Key);
    }

    public EntityMonobeh GenEntity(int x, int z, SeedData seed)
    {
        var pr1 = Perlin(x, z, seed.MapNoise, PerlinHighScale);

        var lh = _layersHigh.FirstOrDefault(l => l.IsInside(pr1));
        var dif = lh.GetFif(pr1);

        var pr2 = Perlin(x, z, seed.MapLayer2, PerlinToxicScale);
        var lt = lh.GetByPerlin(pr2);
        var dif2 = lt.GetDif(pr2);

//        var totalDif = (pr1 + pr2 - lh.MinVal + lt.MinVal) / (lh.MaxVal + lt.MaxVal - lh.MinVal + lt.MinVal);

        return lt.GenEntity(dif2);
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

    public override List<EntityData> GenerateEntitiesByChunk(List<WorldTileData> chunk, SeedData seed)
    {
        List<EntityData> result = new();
        if (!EnableEntityGenerate)
        {
            return result;
        }

        foreach (var t in chunk)
        {
            if (result.Count >= _entitiesByChunk)
            {
                break;
            }

            var resEnt = GenEntity(t.Xpos, t.Zpos, seed);
            if (resEnt != null)
            {
                var halfTile = Config.TileSize / 2;
                result.Add(resEnt.CreateEntity(t.Xpos * Config.TileSize + SimpleExtensions.GetRandom(-halfTile, halfTile),
                    t.Zpos * Config.TileSize + SimpleExtensions.GetRandom(-halfTile, halfTile)));
            }
        }

        return result;
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

        public float GetFif(float pr)
        {
            return (pr - MinVal) / (MaxVal - MinVal);
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
        public TextureEntity BorderTexture;
        public BiomByLayerPerlinGeneration Biom;

        public bool IsInside(float pr)
        {
            return pr >= MinVal && pr <= MaxVal;
        }

        public float GetDif(float pr)
        {
            return (pr - MinVal) / (MaxVal - MinVal);
        }

        public EntityMonobeh GenEntity(float pr)
        {
//            var dif = (pr - MinVal) / (MaxVal - MinVal);

            return Biom.GetRndEntity(pr);
        }
    }
}