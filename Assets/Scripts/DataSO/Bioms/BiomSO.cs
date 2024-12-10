using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(menuName = "Bioms/Base biom", order = 1)]
public class BiomSO : ScriptableObject
{
    public string Key;
    public List<TextureEntity> _textures;

    public List<EntityInBiom> Entities = new();

    internal virtual EntityData GenEntity(TypeGeneration type, int x, int z)
    {
        return null;
    }

    public List<EntityData> GenerateEntitiesByChunk(List<WorldTileData> chunk, SeedData seed)
    {
        List<EntityData> result = new();

        List<WorldTileData> tempPoint = chunk.Where(t => t.Id > 0).ToList();
//        var oneDigital = int.Parse(seed.MapNoiseFrom10To99);
        if (Random.Range(0, 100) < 40)
        {
            AddEntByIndex(result, tempPoint, 6);
        }/**/
        //trees
//        AddEntByIndex(result, tempPoint, 2);
        AddEntByIndex(result, tempPoint, 3);
        AddEntByIndex(result, tempPoint, 4);
        AddEntByIndex(result, tempPoint, 3);
        AddEntByIndex(result, tempPoint, 4);
        AddEntByIndex(result, tempPoint, 4);
        AddEntByIndex(result, tempPoint, 0);
        AddEntByIndex(result, tempPoint, 0);
        AddEntByIndex(result, tempPoint, 1);
        AddEntByIndex(result, tempPoint, 1);

        return result;
    }

    private void AddEntByIndex(List<EntityData> ents, List<WorldTileData> tempPoint, int index)
    {
        if (tempPoint.Count > 0 && Entities.Count > index)
        {
            var tempBlackIds = Entities[index].BlackList.Select(x => x.Id).ToList();
            var tempRandPoint = tempPoint.Where(x => !tempBlackIds.Contains(x.Id)).ToList();
            if (tempRandPoint.Count == 0)
            {
                return;
            }
            var randPoint = tempRandPoint.GetRandom(tempPoint[0].Xpos * tempPoint[0].Zpos + tempPoint.Count + index);

            var halfTile = Config.TileSize / 2;

            var xEntPos = randPoint.Xpos * Config.TileSize + SimpleExtensions.GetRandom(-halfTile, halfTile);
            var zEntPos = randPoint.Zpos * Config.TileSize + SimpleExtensions.GetRandom(-halfTile, halfTile);

            var newEnt = Entities[index].Entity.CreateEntity(xEntPos, zEntPos);
            ents.Add(newEnt);
            tempPoint.Remove(randPoint);
        }
    }
}

[System.Serializable]
public class EntityInBiom
{
    public EntityMonobeh Entity;
    public int Weight;
    public List<TextureEntity> BlackList;
}