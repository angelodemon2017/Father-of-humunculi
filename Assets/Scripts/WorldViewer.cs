using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Unity.AI.Navigation;
using UnityEngine;

public class WorldViewer : MonoBehaviour
{
    public static WorldViewer Instance;

    [SerializeField] private NavMeshSurface _navMeshSurface;
    [SerializeField] private BasePlaneWorld _basePlaneWorld;

    [SerializeField] private List<TextureEntity> _textureEntities = new();

    private WorldData _gameWorld;
    [SerializeField] private List<WorldChunkView> _chunksView = new();//TODO remove SerializeField

    private Vector3 _focusChunkPosition = Vector3.back;
    [SerializeField] private List<Vector3> _chunkPoints = new();//TODO remove SerializeField

    public List<TextureEntity> Textures => _textureEntities;

    private void Awake()
    {
        Instance = this;
        InitWorld(new WorldData());
    }

    private void OnDrawGizmos()
    {
        foreach (var p in _chunksView)
        {
            Gizmos.DrawSphere(p.ChunkPosition, 1);
        }
    }

    public void RegenerateWorld()
    {
        InitWorld(new WorldData());
        foreach (var c in _chunksView)
        {
            c.CleanChunk();
        }
        _chunksView.Clear();
        CheckAndUpdateChunks();
    }

    public void InitWorld(WorldData data)
    {
        _gameWorld = data;
    }

    private void Update()
    {
        UpdateCenterUpdate(CameraController.Instance.FocusPosition);
    }

    public void UpdateCenterUpdate(Vector3 newCenter)
    {
        var newPos = (Vector3)Vector3Int.RoundToInt(newCenter / Config.ChunkSize) * Config.ChunkSize;

        if (newPos != _focusChunkPosition)
        {
            _focusChunkPosition = newPos;
            CheckAndUpdateChunks();
        }
    }

    private void CheckAndUpdateChunks()
    {
        _chunkPoints.Clear();
        for (int x = -Config.VisibilityChunkDistance; x < Config.VisibilityChunkDistance; x++)
            for (int z = -Config.VisibilityChunkDistance; z < Config.VisibilityChunkDistance; z++)
            {
                _chunkPoints.Add(new Vector3(_focusChunkPosition.x + x * Config.ChunkSize, 0f,
                    _focusChunkPosition.z + z * Config.ChunkSize));
            }

        List<WorldChunkView> chunkForDelete = new();
        List<Vector3> exceptChunk = new();
        var distanceVisible = Config.VisibilityChunkDistance * Config.ChunkSize;
        foreach (var chunk in _chunksView)
        {
            if (chunk.ChunkPosition.x >= _focusChunkPosition.x + distanceVisible ||
                chunk.ChunkPosition.x <= _focusChunkPosition.x - distanceVisible ||
                chunk.ChunkPosition.z >= _focusChunkPosition.z + distanceVisible ||
                chunk.ChunkPosition.z <= _focusChunkPosition.z - distanceVisible)
            {
                chunkForDelete.Add(chunk);
            }
            else
            {
                exceptChunk.Add(chunk.ChunkPosition);
            }
        }

        foreach (var chunk in chunkForDelete)
        {
            chunk.CleanChunk();
            _chunksView.Remove(chunk);
        }

        foreach (var point in exceptChunk)
        {
            _chunkPoints.Remove(point);
        }

        foreach (var point in _chunkPoints)
        {
            _chunksView.Add(new WorldChunkView(point, _gameWorld, Create));
        }

        _navMeshSurface.RemoveData();

        _navMeshSurface.BuildNavMesh();
    }

    public void LoadAndViewChunk()
    {
//        var qwe = new WorldChunkView(Vector3.zero, _gameWorld.worldTileDatas, Create);
    }

    private BasePlaneWorld Create()
    {
        return Instantiate(_basePlaneWorld, transform);
    }
}

[System.Serializable]
public class WorldChunkView
{
    public Vector3 ChunkPosition;

    public BasePlaneWorld[,] viewTiles;

    public WorldChunkView(Vector3 position, WorldData worldData, Func<BasePlaneWorld> creating)
    {
        ChunkPosition = position;
        viewTiles = new BasePlaneWorld[Config.ChunkTilesSize, Config.ChunkTilesSize];
        var worldParts = worldData.GetChunk((int)(position.x / Config.ChunkSize), (int)(position.z / Config.ChunkSize));
        for (int x=0; x< Config.ChunkTilesSize; x++)
            for (int z = 0; z < Config.ChunkTilesSize; z++)
            {
                var bpw = creating.Invoke();
                var wt = new WorldTile(worldParts[x * Config.ChunkTilesSize + z]);
                var neigbors = worldData.GetNeigborsTiles(wt.Xpos, wt.Zpos).Select(t => new WorldTile(t)).ToList();
                bpw.Init(wt, neigbors);
                viewTiles[x, z] = bpw;
            }
    }

    public void CleanChunk()
    {
        for (int x = 0; x < Config.ChunkTilesSize; x++)
            for (int z = 0; z < Config.ChunkTilesSize; z++)
            {
                GameObject.Destroy(viewTiles[x, z].gameObject);
            }
    }
}