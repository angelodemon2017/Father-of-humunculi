using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Bioms/Biom of layer", order = 2)]
public class BiomByLayerPerlinGeneration : BiomSO
{
    [SerializeField] private List<LayerOfEntity> _entities = new();

    internal override EntityData GenEntity(TypeGeneration type, int x, int z)
    {
        var halfTile = Config.TileSize / 2;
        return _entities[0].Entity.CreateEntity(x * Config.TileSize + SimpleExtensions.GetRandom(-halfTile, halfTile), z * Config.TileSize + SimpleExtensions.GetRandom(-halfTile, halfTile));
    }

    public EntityMonobeh GetRndEntity(float pr)
    {
        if (_entities.Count == 0)
        {
            return null;
        }

        return _entities.FirstOrDefault(e => e.IsInside(pr)).Entity;
    }

    [System.Serializable]
    internal class LayerOfEntity
    {
        [Range(0, 1)]
        public float MinDeep = 0;
        [Range(0, 1)]
        public float MaxDeep = 1;

        public EntityMonobeh Entity;

        public bool IsInside(float pr)
        {
            return pr >= MinDeep && pr <= MaxDeep;
        }
    }
}