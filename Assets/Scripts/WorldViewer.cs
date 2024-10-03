using System;
using System.Collections.Generic;
using System.Linq;
using Unity.AI.Navigation;
using UnityEngine;

public class WorldViewer : MonoBehaviour
{
    [SerializeField] private NavMeshSurface _navMeshSurface;
    [SerializeField] private BasePlaneWorld _basePlaneWorld;

    [SerializeField] private List<TextureEntity> _textureEntities = new();

    private WorldData _gameWorld;
    private List<WorldChunkView> chunksView = new();

    private Vector3 _centerUpdate;
    private float _visibilityDistance;
    private float _tileSize = 1f;

    private float ChunkStep => _tileSize * WorldChunkView.ChunkSize;

    public void UpdateCenterUpdate(Vector3 newCenter)
    {
        _centerUpdate = newCenter;

    }

    public void LoadAndViewChunk()
    {
        var qwe = new WorldChunkView(Vector3.zero, _gameWorld.worldTileDatas, Create);
    }

    private BasePlaneWorld Create()
    {
        return Instantiate(_basePlaneWorld, transform);
    }
}

public class WorldChunkView
{
    public static int ChunkSize = 3;
    public Vector3 ChunkPosition;

    public BasePlaneWorld[,] viewTiles;

    public WorldChunkView(Vector3 position, List<WorldTileData> worldParts, Func<BasePlaneWorld> creating)
    {
        ChunkPosition = position;
        worldParts = worldParts.OrderBy(p => p.Zpos).OrderBy(p => p.Xpos).ToList();
        for(int x=0; x< ChunkSize; x++)
            for (int z = 0; z < ChunkSize; z++)
            {
                var bpw = creating.Invoke();
//                bpw.Init(worldParts[x + z * ChunkSize], );
                viewTiles[x, z] = bpw;
            }
    }
}