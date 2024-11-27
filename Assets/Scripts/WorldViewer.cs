using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WorldViewer : MonoBehaviour
{
    public static WorldViewer Instance;

    public bool DebugMode;
//    public NavMeshSurfaceVolumeUpdater _updaterOnScene;
    [SerializeField] private NavMeshSurfaceVolumeUpdater _navMeshUpdaterPrefab;
//    [SerializeField] private NavMeshSurface _navMeshSurface;
    [SerializeField] private BasePlaneWorld _basePlaneWorld;
    [SerializeField] private EntityMonobeh _entityMonobehPrefab;
    [SerializeField] private Transform _entityParent;

    [SerializeField] private List<TextureEntity> _textureEntities = new();
    [SerializeField] private EntitiesLibrary entitiesLibrary;

    private List<WorldChunkView> _chunksView = new();
    private Dictionary<(int, int), WorldTile> _cashTiles = new();
    private List<EntityMonobeh> _cashEntities = new();

    private Vector3 _focusChunkPosition;
    private List<Vector3> _chunkPoints = new();

    public EntityMonobeh GetEM(long id) => _cashEntities.FirstOrDefault(e => e.Id == id);
    public List<TextureEntity> Textures => _textureEntities;
    private WorldData _gameWorld => GameProcess.Instance.GameWorld;

    private HashSet<BasePlaneWorld> _poolBPW = new();
    private Dictionary<string, HashSet<EntityMonobeh>> _poolEntities = new();
    private HashSet<NavMeshSurfaceVolumeUpdater> _poolSurfacers = new();


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
            newBPWs.ForEach(x => _poolBPW.Add(x));
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
        _cashEntities.ForEach(e => cashOfCash.Add(e));
        foreach (var chunk in chunkForDelete)
        {
            var entsForDel = cashOfCash.Where(e => e.transform.position.GetChunkPos() == chunk.ChunkPosition).ToList();
            foreach (var ent in entsForDel)
            {
                RemoveEntity(ent);
            }
            var newBPWs = chunk.CleanChunk();
            newBPWs.ForEach(x => _poolBPW.Add(x));
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

        var newEM = Create(entityInProcess.EntityData.TypeKey);
        newEM.Init(entityInProcess);
        _cashEntities.Add(newEM);
    }

    public void RemoveEntity(EntityMonobeh entityMonobeh)
    {
        _cashEntities.Remove(entityMonobeh);

        if (_poolEntities.TryGetValue(entityMonobeh.EntityInProcess.EntityData.TypeKey, out HashSet<EntityMonobeh> pool))
        {
            pool.Add(entityMonobeh);
        }
        else
        {
            _poolEntities.Add(entityMonobeh.EntityInProcess.EntityData.TypeKey, new());
            _poolEntities[entityMonobeh.EntityInProcess.EntityData.TypeKey].Add(entityMonobeh);
        }
        entityMonobeh.VirtualDestroy();
    }

    private EntityMonobeh Create(string key)
    {
        if (_poolEntities.ContainsKey(key) && _poolEntities[key].Count > 0)
        {
            var tempEMB = _poolEntities[key].ElementAt(0);
            _poolEntities[key].Remove(tempEMB);
            tempEMB.VirtualCreate();
            return tempEMB;
        }

        var entMon = entitiesLibrary.GetConfig(key);
        return Instantiate(entMon, _entityParent);
    }

    public void Remove(NavMeshSurfaceVolumeUpdater nmsvu)
    {
        nmsvu.VirtualDestroy();
        _poolSurfacers.Add(nmsvu);
    }

    public NavMeshSurfaceVolumeUpdater GetUpdater()
    {
        if (_poolSurfacers.Count > 0)
        {
            var tempPS = _poolSurfacers.ElementAt(0);
            _poolSurfacers.Remove(tempPS);
            tempPS.VirtualCreate();
            return tempPS;
        }

        return Instantiate(_navMeshUpdaterPrefab, transform);
    }

    private BasePlaneWorld Create()
    {
        if (_poolBPW.Count > 0)
        {
            var tempBPW = _poolBPW.ElementAt(0);
            _poolBPW.Remove(tempBPW);
            tempBPW.VirtualCreate();
            return tempBPW;
        }

        return Instantiate(_basePlaneWorld, transform);
    }
}

[System.Serializable]
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