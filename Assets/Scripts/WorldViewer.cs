using System;
using System.Collections.Generic;
using System.Linq;
using Unity.AI.Navigation;
using UnityEngine;

public class WorldViewer : MonoBehaviour
{
    public static WorldViewer Instance;

    public bool DebugMode;
    [SerializeField] private NavMeshSurface _navMeshSurface;
    [SerializeField] private BasePlaneWorld _basePlaneWorld;
    [SerializeField] private EntityMonobeh _entityMonobehPrefab;

    [SerializeField] private List<TextureEntity> _textureEntities = new();

    [SerializeField] private List<WorldChunkView> _chunksView = new();
    private Dictionary<(int, int), WorldTile> _cashTiles = new();
    [SerializeField] private List<EntityMonobeh> _cashEntities = new();

    private Vector3 _focusChunkPosition;
    private List<Vector3> _chunkPoints = new();

    public List<TextureEntity> Textures => _textureEntities;
    private WorldData _gameWorld => GameProcess.Instance.GameWorld;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        GameplayAdapter.Instance.Newgame();
        CheckAndUpdateChunks();
    }

    private void OnDrawGizmos()
    {
        foreach (var p in _chunksView)
        {
            Gizmos.DrawSphere(p.ChunkPosition, 1);
        }

        var sizeGizmo = Vector3.one * Config.ChunkSize;
        sizeGizmo.y = 1;

        Gizmos.DrawCube(_focusChunkPosition, sizeGizmo);
    }

    public WorldTile GetWorldTile(int x, int z)
    {
        if (_cashTiles.TryGetValue((x, z), out WorldTile tile))
        {
            return tile;
        }

        var wtd = new WorldTile(_gameWorld.GetWorldTileData(x,z));

        _cashTiles.Add((x, z), wtd);

        return wtd;
    }

    public TextureEntity GetTE(int id)
    {
        return _textureEntities.FirstOrDefault(x => x.Id == id);
    }

    public void RegenerateWorld()
    {
        _cashTiles.Clear();
        foreach (var c in _chunksView)
        {
            c.CleanChunk();
        }
        _chunksView.Clear();

        foreach (var e in _cashEntities)
        {
            Destroy(e.gameObject);
        }
        _cashEntities.Clear();

        GameProcess.Instance.NewGame(new WorldData());
        CheckAndUpdateChunks();
    }

    [SerializeField] private LayerMask _mask;
    private RaycastHit hit;

    private void Update()
    {
        UpdateCenterUpdate(CameraController.Instance.FocusPosition);
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); 
            if (Physics.Raycast(ray, out hit, 100, _mask))
            {
                if (hit.transform.TryGetComponent(out BasePlaneWorld comp))
                {
                    comp.ChangeTextureRandom();
                }
            }
        }
    }

    public void UpdateCenterUpdate(Vector3 newCenter)
    {
        var newPos = newCenter.GetChunkPos();

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

        var cashOfCash = new List<EntityMonobeh>();
        _cashEntities.ForEach(e => cashOfCash.Add(e));
        foreach (var chunk in chunkForDelete)
        {
            var entsForDel = cashOfCash.Where(e => e.transform.position.GetChunkPos() == chunk.ChunkPosition).ToList();
            foreach (var ent in entsForDel)
            {
                Destroy(ent.gameObject);
                _cashEntities.Remove(ent);
            }
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
            var eips = GameProcess.Instance.GetEntitiesByChunk((int)point.x, (int)point.z);

            if (eips.Count != Config.EntitiesInChunk)
            {
                Debug.Log($"ViewChunk(). Entities:{eips.Count()}");

                GameProcess.Instance.GetEntitiesByChunk((int)point.x, (int)point.z);
            }
            foreach (var eip in eips)
            {
                var newEM = Instantiate(_entityMonobehPrefab);
                newEM.Init(eip);
                _cashEntities.Add(newEM);
            }
        }

        _navMeshSurface.RemoveData();

        _navMeshSurface.BuildNavMesh();
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
                var wt = WorldViewer.Instance.GetWorldTile(worldParts[x * Config.ChunkTilesSize + z].Xpos,
                    worldParts[x * Config.ChunkTilesSize + z].Zpos);
                var neigbors = worldData.GetNeigborsTiles(wt.Xpos, wt.Zpos).Select(t => WorldViewer.Instance.GetWorldTile(t.Xpos, t.Zpos)).ToList();
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