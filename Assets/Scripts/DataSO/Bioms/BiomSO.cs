using System.Collections.Generic;
using UnityEngine;
using System;
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
        if (tempPoint.Count > 0)
        {
            var randPoint = tempPoint.GetRandom();

            var xEntPos = randPoint.Xpos * Config.TileSize;
            var zEntPos = randPoint.Zpos * Config.TileSize;

            var newEnt = Entities.FirstOrDefault().Entity.InitEntity(xEntPos, zEntPos);
            result.Add(newEnt);
        }

        return result;
    }
}

[Serializable]
public class EntityInBiom
{
    public EntitySO Entity;
    public int Weight;
}