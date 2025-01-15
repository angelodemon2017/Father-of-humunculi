﻿using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using static Helper;

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
    [SerializeField] private List<Structure> _structures = new();

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

    public override List<EntityData> GenerateEntitiesByChunk(List<WorldTileData> chunk, WorldData wd)
    {
        List<EntityData> result = new();
        var tempPos = chunk[0].GetChunkPos;

        result.AddRange(CheckStructures(new Vector3Int(tempPos.Item1, 0, tempPos.Item2), wd.Seed));

        HashSet<EntityData> allNeigs = new();
        for (int x = -1; x < 2; x++)
            for (int z = -1; z < 2; z++)
            {
                var tempRes = wd.GetEntitiesByChunk(tempPos.Item1, tempPos.Item2);
                foreach (var te in tempRes)
                {
                    allNeigs.Add(te);
                }
            }

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

            var resEnt = GenEntity(t.Xpos, t.Zpos, wd.Seed);
            if (resEnt != null)
            {
                var halfTile = Config.TileSize / 2;
                var xPos = t.Xpos * Config.TileSize + SimpleExtensions.GetRandom(-halfTile, halfTile);
                var zPos = t.Zpos * Config.TileSize + SimpleExtensions.GetRandom(-halfTile, halfTile);

                var tempEntResult = resEnt.CreateEntity(xPos, zPos);
                bool missCreate = false;

                foreach (var ne in allNeigs)
                {
                    if (ne.IsTooClose(tempEntResult))
                    {
//                        Debug.Log("Miss add Entity");
                        missCreate = true;
                    }
                }
                if (missCreate)
                {
                    continue;
                }

                allNeigs.Add(tempEntResult);
                result.Add(tempEntResult);
            }
        }

        return result;
    }

    private List<EntityData> CheckStructures(Vector3Int ChunkPos, SeedData seed)
    {
        List<EntityData> result = new();



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

    [System.Serializable]
    internal class Structure
    {
        public EntityMonobeh EntityOfStructure;
        public TextureEntity GroundUnderStructure;
        public int Limit = 1;
        public float Distance = 10f;
        public int AddSwiftAngleStructure = 0;

        private Vector3 GetPos(int numstruct, SeedData seed)
        {
            var angle = 360 / Limit * numstruct + AddSwiftAngleStructure + seed.StructureAngleSwift;

            return PointOnCircumference(0f, 0f, Distance, angle);
        }

        public bool CheckCoordinate(Vector3Int ChunkPos, SeedData seed)
        {
            for (int i = 0; i < Limit; i++)
            {
                var tempPos = GetPos(i, seed);
                if (tempPos.GetChunkPosInt() == ChunkPos)
                {
                    return true;
                }
            }

            return false;
        }
    }
}