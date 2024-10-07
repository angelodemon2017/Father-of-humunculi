using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using System.Linq;

public class WorldGenerator : MonoBehaviour
{
    public static WorldGenerator Instance;

    [SerializeField] private NavMeshSurface _navMeshSurface;
    [SerializeField] private BasePlaneWorld _basePlaneWorld;

    [SerializeField] int _widthMap;
    [SerializeField] int _lengthMap;

    [SerializeField] private List<TextureEntity> _textureEntities = new();

    public Vector2 WorldSwift;

    private WorldConstructor _world = new();
    public TextureEntity GetTE(int id) => _textureEntities.FirstOrDefault(x => x.Id == id);

    private void Awake()
    {
        Instance = this;
//        GenerationMap();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            _navMeshSurface.RemoveData();

            _navMeshSurface.BuildNavMesh();
        }
    }

    public void GenerationMap()
    {
        _navMeshSurface.RemoveData();

        transform.DestroyChildrens();

        GenerateWorldEntity();

        GenerateWorldCanvas();

        _navMeshSurface.BuildNavMesh();
    }

    private void GenerateWorldEntity()
    {
        _world.Generate(_widthMap, _lengthMap, "", _textureEntities);
    }

    private void GenerateWorldCanvas()
    {
        for (int x = 0; x < _widthMap; x++)
            for (int z = 0; z < _lengthMap; z++)
            {
                var bpw = Instantiate(_basePlaneWorld, transform);
                bpw.Init(_world.partGrid[x, z], GetNeigborsTiles(x, z));
            }
    }

    public List<WorldTile> GetNeigborsTiles(int X, int Z)
    {
        List<WorldTile> wps = new();
        for (int x = -1; x < 2; x++)
            for (int z = -1; z < 2; z++)
            {
                var xpos = X + x;
                var zpos = Z + z;
                if (x == 0 && z == 0 || 
                    xpos < 0 || zpos < 0 || xpos >= _widthMap || zpos >= _lengthMap)
                {
                    continue;
                }
                wps.Add(_world.partGrid[xpos, zpos]);
            }

        return wps;
    }
}