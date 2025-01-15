using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WorldViewer : MonoBehaviour
{
    public static WorldViewer Instance;

    public bool DebugMode;
    [SerializeField] private PowerDecoration _prefabDecoration;
    [SerializeField] private NavMeshSurfaceVolumeUpdater _navMeshUpdaterPrefab;
    [SerializeField] private BasePlaneWorld _basePlaneWorld;
    [SerializeField] private EntityMonobeh _entityMonobehPrefab;
    [SerializeField] private Transform _entityParent;

    [SerializeField] private List<TextureEntity> _textureEntities = new();
    [SerializeField] private EntitiesLibrary entitiesLibrary;

    private List<WorldChunkView> _chunksView = new();
    private Dictionary<(int, int), WorldTile> _cashTiles = new();
    private HashSet<EntityMonobeh> _cashEntities = new();
    public static Action<int> CashEntitiesCount;

    private Vector3 _focusChunkPosition;
    public static Action<Vector3> FocusChunkChanged;
    private HashSet<Vector3> _chunkPoints = new();

//    public EntityMonobeh GetEM(long id) => _cashEntities.FirstOrDefault(e => e.Id == id);
    public List<TextureEntity> Textures => _textureEntities;
    private WorldData _gameWorld => GameProcess.Instance.GameWorld;

    private ObjectPool<BasePlaneWorld> _poolBPW;
    private EntityMonobehFabric _entityMonobehFabric;
    private ObjectPool<NavMeshSurfaceVolumeUpdater> _poolSurfacers;
    private ObjectPool<PowerDecoration> _poolPD;

    private CashDictionary<Vector3Int, CenterDecors> _cashCentDecs = new();
    private CashDictionary<Vector3Int, PowerDecoration> _cashPowerDecs = new();

    internal CashDictionary<Vector3Int, CenterDecors> CashCentDecs => _cashCentDecs;
    internal CashDictionary<Vector3Int, PowerDecoration> CashPowerDecs => _cashPowerDecs;
    internal ObjectPool<NavMeshSurfaceVolumeUpdater> PoolSurfacers => _poolSurfacers;
    internal ObjectPool<PowerDecoration> PoolPD => _poolPD;

    private void Awake()
    {
        Instance = this;
        PoolsInit();
    }

    private void PoolsInit()
    {
        _poolBPW = new ObjectPool<BasePlaneWorld>(_basePlaneWorld, transform);
        _poolSurfacers = new ObjectPool<NavMeshSurfaceVolumeUpdater>(_navMeshUpdaterPrefab, transform);
        _entityMonobehFabric = new EntityMonobehFabric(entitiesLibrary, _entityParent);
        _poolPD = new ObjectPool<PowerDecoration>(_prefabDecoration, transform);
    }

    private void Start()
    {
        GameplayAdapter.Instance.Newgame();
        CheckAndUpdateChunks();
    }

    private void OnDrawGizmos()
    {
        if (!DebugMode)
        {
            return;
        }

        foreach (var p in _chunksView)
        {
            Gizmos.DrawSphere(p.ChunkPosition, 1);
        }

        var sizeGizmo = Vector3.one * Config.ChunkSize;
        sizeGizmo.y = 1;

        Gizmos.DrawCube(_focusChunkPosition, sizeGizmo);
    }

    public WorldTile GetWorldTile(WorldTileData tileData)
    {
        if (!_cashTiles.TryGetValue((tileData.Xpos, tileData.Zpos), out WorldTile tile))
        {
            tile = new WorldTile(tileData);
            _cashTiles.Add((tileData.Xpos, tileData.Zpos), tile);
        }

        return tile;
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
            var newBPWs = c.CleanChunk();
            newBPWs.ForEach(x => _poolBPW.DestroyObject(x));
//            _poolBPW.Add(x)); ;
        }
        _chunksView.Clear();

        foreach (var e in _cashEntities)
        {
            Destroy(e.gameObject);
        }
        _cashEntities.Clear();
        CashEntitiesCount?.Invoke(_cashEntities.Count);

        GameProcess.Instance.NewGame(new WorldData());
        CheckAndUpdateChunks();
    }

    private void Update()
    {
        UpdateCenterUpdate(CameraController.Instance.FocusPosition);
    }

    public void UpdateCenterUpdate(Vector3 newCenter)
    {
        var newPos = newCenter.GetChunkPos();

        if (newPos != _focusChunkPosition)
        {
            _focusChunkPosition = newPos;
            FocusChunkChanged?.Invoke(_focusChunkPosition);

            CheckAndUpdateChunks();

            StartCoroutine(UIPanelMinimap.Instance.Init());
        }
    }

    private void CheckAndUpdateChunks()
    {
        List<Vector3> chunkPreGenerate = new();
        int q = 5;
        for (int x = -Config.VisibilityChunkDistance - q; x < Config.VisibilityChunkDistance + 1 + q; x++)
            for (int z = -Config.VisibilityChunkDistance - q; z < Config.VisibilityChunkDistance + 1 + q; z++)
            {
                chunkPreGenerate.Add(new Vector3(_focusChunkPosition.x + x * Config.ChunkSize, 0f,
                    _focusChunkPosition.z + z * Config.ChunkSize));
            }
        _chunkPoints.Clear();
        for (int x = -Config.VisibilityChunkDistance + 1; x < Config.VisibilityChunkDistance; x++)
            for (int z = -Config.VisibilityChunkDistance + 1; z < Config.VisibilityChunkDistance; z++)
            {
                var newP = new Vector3(_focusChunkPosition.x + x * Config.ChunkSize, 0f,
                    _focusChunkPosition.z + z * Config.ChunkSize);

                _chunkPoints.Add(newP);
                chunkPreGenerate.Remove(newP);
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
        foreach (var ent in _cashEntities)
        {
            cashOfCash.Add(ent);
        }
        foreach (var chunk in chunkForDelete)
        {
            var entsForDel = cashOfCash.Where(e => e.transform.position.GetChunkPos() == chunk.ChunkPosition).ToList();
            foreach (var ent in entsForDel)
            {
                RemoveEntity(ent);
            }
            var newBPWs = chunk.CleanChunk();
            foreach (var bpw in newBPWs)
            {
                _poolBPW.DestroyObject(bpw);
            }
            _chunksView.Remove(chunk);
        }
        foreach (var point in exceptChunk)
        {
            _chunkPoints.Remove(point);
        }

        foreach (var point in _chunkPoints)
        {
            _chunksView.Add(new WorldChunkView(point, _gameWorld, _poolBPW.Get));
            var eips = GameProcess.Instance.GetEntitiesByChunk((int)point.x, (int)point.z);

            foreach (var eip in eips)
            {
                TryAddEntity(eip);
            }
        }

        foreach (var c in chunkPreGenerate)
        {
            StartCoroutine(GameProcess.Instance.GameWorld.CheckAndGenChunk((int)(c.x / Config.ChunkSize), (int)(c.z / Config.ChunkSize)));
        }
    }

    public void TryAddEntity(EntityInProcess entityInProcess)
    {
        var viewDistance = Config.VisibilityChunkDistance * Config.ChunkSize - Config.ChunkSize / 2;
        if (entityInProcess.Position.x < _focusChunkPosition.x - viewDistance ||
            entityInProcess.Position.x > _focusChunkPosition.x + viewDistance ||
            entityInProcess.Position.z < _focusChunkPosition.z - viewDistance ||
            entityInProcess.Position.z > _focusChunkPosition.z + viewDistance ||
            _cashEntities.Any(x => x.Id == entityInProcess.Id))
        {
            return;
        }

        var newEM = _entityMonobehFabric.Create(entityInProcess.EntityData.TypeKey);
        newEM.Init(entityInProcess);
        _cashEntities.Add(newEM);
        CashEntitiesCount?.Invoke(_cashEntities.Count);
    }

    public void RemoveEntity(EntityMonobeh entityMonobeh)
    {
        _cashEntities.Remove(entityMonobeh);
        CashEntitiesCount?.Invoke(_cashEntities.Count);

        _entityMonobehFabric.DestroyEntity(entityMonobeh);
    }
}

[Serializable]
public class WorldChunkView
{
    public Vector3 ChunkPosition;

    private List<BasePlaneWorld> _cashTiles = new();

    public WorldChunkView(Vector3 position, WorldData worldData, Func<BasePlaneWorld> creating)
    {
        ChunkPosition = position;
        var worldParts = worldData.GetChunk((int)(position.x / Config.ChunkSize), (int)(position.z / Config.ChunkSize));

        foreach (var wp in worldParts)
        {
            var bpw = creating.Invoke();
            var wt = WorldViewer.Instance.GetWorldTile(wp);

            var neigbors = worldData.GetNeigborsTiles(wt.Xpos, wt.Zpos).Select(t => WorldViewer.Instance.GetWorldTile(t)).ToList();
            bpw.Init(wt, neigbors);
            _cashTiles.Add(bpw);
        }
    }

    public List<BasePlaneWorld> CleanChunk()
    {
        _cashTiles.ForEach(t => t.VirtualDestroy());
        return _cashTiles;
    }
}