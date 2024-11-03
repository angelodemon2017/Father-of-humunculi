using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "BiomSO", order = 1)]
public class BiomSO : ScriptableObject
{
    public string Key;
    public List<TextureEntity> _textures;

    public List<EntityInBiom> Entities = new();
}

[Serializable]
public class EntityInBiom
{
    public EntitySO Entity;
    public int Weight;
}