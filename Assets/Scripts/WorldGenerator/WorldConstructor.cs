using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WorldConstructor
{
    public static WorldTileData GenerateTile(int x, int z, SeedData seed)
    {
        var pr = Mathf.PerlinNoise((x - seed.PerlinNoiseSwift) * Config.PerlinScale, (z + seed.PerlinNoiseSwift) * Config.PerlinScale);
        var index = pr * WorldViewer.Instance.Textures.Count + Config.HolesMap;
        if (index >= WorldViewer.Instance.Textures.Count)
        {
            index = WorldViewer.Instance.Textures.Count - 1;
        }
        if (index < 0)
        {
            index = 0;
        }
        var txt = WorldViewer.Instance.Textures[(int)index];
        return new WorldTileData(txt.Id, x, z);
    }

    public static List<EntityData> GenerateEntitiesByChunk(int x, int z, List<WorldTileData> chunk)
    {
        List<EntityData> result = new();
        List<WorldTileData> tempPoint = chunk.Where(t => t.Id > 0).ToList();
        var pointForGen = GetRandomTiles(tempPoint, tempPoint.Count > Config.EntitiesInChunk ? Config.EntitiesInChunk : tempPoint.Count);

        //        var chunkPos = new UnityEngine.Vector3(x * Config.ChunkSize, 0, z * Config.ChunkSize);

        foreach (var ed in pointForGen)
        {
            var xEntPos = ed.Xpos * Config.TileSize;
            var zEntPos = ed.Zpos * Config.TileSize;

            var random = Random.Range(0, 10);

            if (random > 5)
            {
                var newEnt = new EntityGoldBush(xEntPos, zEntPos);
                result.Add(newEnt);
            }
            else
            {
                var newEnt = new EntityResource(ed.Id < 3 ? 0 : 1, xEntPos, zEntPos);
                result.Add(newEnt);
            }
        }

        pointForGen = GetRandomTiles(tempPoint, 1);

        foreach (var ed in pointForGen)
        {
            var xEntPos = ed.Xpos * Config.TileSize;
            var zEntPos = ed.Zpos * Config.TileSize;

            var newEnt = new EntityItem((EnumItem)Random.Range(1,3), xEntPos, zEntPos);

            result.Add(newEnt);
        }

        return result;
    }

    private static List<WorldTileData> GetRandomTiles(List<WorldTileData> tempPoint, int count)
    {
        var pointForGen = new List<WorldTileData>();

        for (int p = 0; p < count; p++)
        {
            if (tempPoint.Count == 0)
            {
                break;
            }
            var tp = tempPoint.GetRandom();
            pointForGen.Add(tp);
            tempPoint.Remove(tp);
        }

        return pointForGen;
    }
}