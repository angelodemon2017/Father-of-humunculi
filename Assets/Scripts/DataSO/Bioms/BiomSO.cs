using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "BiomSO", order = 1)]
public class BiomSO : ScriptableObject
{
    public string Key;
    public List<TextureEntity> _textures;

    public List<EntityInBiom> Entities = new();

    public List<EntityData> GenerateEntitiesByChunk(List<WorldTileData> chunk)
    {
        List<EntityData> result = new();



        return result;
    }
}

[Serializable]
public class EntityInBiom
{
    public EntitySO Entity;
    public int Weight;
}