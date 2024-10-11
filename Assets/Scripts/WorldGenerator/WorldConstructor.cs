using System.Collections.Generic;
using System.Linq;

public class WorldConstructor
{
    const int minimumEntitiesByChunk = 2;

    public static WorldTileData GenerateTile(int x, int z)
    {
        var txt = WorldViewer.Instance.Textures.GetRandom();
        return new WorldTileData(txt.Id, x, z);
    }

    public static List<EntityData> GenerateEntitiesByChunk(int x, int z, List<WorldTileData> chunk)
    {
        List<EntityData> result = new();
        List<WorldTileData> tempPoint = chunk.Where(t => t.Id > 0).ToList();
        var pointForGen = new List<WorldTileData>();

        int countGens = tempPoint.Count > minimumEntitiesByChunk ? minimumEntitiesByChunk : tempPoint.Count;
        for (int p = 0; p < countGens; p++)
        {
            if (tempPoint.Count == 0)
            {
                break;
            }
            var tp = tempPoint.GetRandom();
            pointForGen.Add(tp);
            tempPoint.Remove(tp);
        }

        foreach (var ed in pointForGen)
        {
            var xEntPos = ed.Xpos * Config.TileSize;
            var zEntPos = ed.Zpos * Config.TileSize;

            result.Add(new EntityResource(ed.Id < 3 ? 0 : 1, xEntPos, zEntPos));
        }

        return result;
    }
}