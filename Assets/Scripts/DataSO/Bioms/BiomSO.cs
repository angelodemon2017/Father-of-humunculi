using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(menuName = "BiomSO", order = 1)]
public class BiomSO : ScriptableObject
{
    public string Key;
    public List<TextureEntity> _textures;

    public List<EntityInBiom> Entities = new();

    public List<EntityData> GenerateEntitiesByChunk(List<WorldTileData> chunk, SeedData seed)
    {
        List<EntityData> result = new();

        List<WorldTileData> tempPoint = chunk.Where(t => t.Id > 0).ToList();
        var oneDigital = int.Parse(seed.MapNoiseFrom10To99);
        if (Random.Range(-10, 110) > oneDigital) 
        {
            AddEntByIndex(result, tempPoint, 0);
        }
        AddEntByIndex(result, tempPoint, 1);
        //trees
        AddEntByIndex(result, tempPoint, 2);
        AddEntByIndex(result, tempPoint, 3);
        AddEntByIndex(result, tempPoint, 3);

        return result;
    }

    private void AddEntByIndex(List<EntityData> ents, List<WorldTileData> tempPoint, int index)
    {
        if (tempPoint.Count > 0 && Entities.Count > index)
        {
            var randPoint = tempPoint.GetRandom(tempPoint[0].Xpos * tempPoint[0].Zpos + tempPoint.Count + index);

            var xEntPos = randPoint.Xpos * Config.TileSize;
            var zEntPos = randPoint.Zpos * Config.TileSize;

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
}