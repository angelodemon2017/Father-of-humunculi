using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using static OptimazeExtensions;

public class BasePlaneWorld : MonoBehaviour
{
    protected int Uid;
    [SerializeField] private Renderer _renderer;
    [SerializeField] private List<Renderer> _borders;
    private WorldTile _worldPart;

    private List<WorldTile> _neigbors = new();
    [SerializeField] private Texture2D _textureBlackMask;
    [SerializeField] private GameObject _collider;
    [SerializeField] private TextMeshProUGUI _testText;

    [SerializeField] private Transform _parentDecorations;
    [SerializeField] private PowerDecoration _prefabDecoration;
    private HashSet<PowerDecoration> _decorations = new();

    public string DebugText;

    internal TextureEntity TextureEntity => WorldViewer.Instance.GetTE(_worldPart.Id);

    private void Awake()
    {
        Uid = PoolCounter<BasePlaneWorld>.NextUid();

        CalcDecorations();
    }

    public void VirtualCreate()
    {
        gameObject.SetActive(true);
        CalcDecorations();
    }

    public void Init(WorldTile worldPart, List<WorldTile> neigbors)
    {
        _worldPart = worldPart;
        _worldPart.ChangedId += UpdateNeigbor;
        foreach (var wp in neigbors)
        {
            _neigbors.Add(wp);
            wp.ChangedId += UpdateNeigbor;
        }
        transform.position = new Vector3(_worldPart.Xpos * Config.TileSize, 0f, _worldPart.Zpos * Config.TileSize);
        UpdatePart();
        DebugInfo();
    }

    private void DebugInfo()
    {
        _testText.transform.parent.gameObject.SetActive(WorldViewer.Instance.DebugMode);
        _testText.text = _worldPart.DebugData;
    }

    private void UpdateNeigbor(int id)
    {
        UpdatePart();
    }

    public void UpdatePart()
    {
        var _textureEntity = TextureEntity;

        _collider.SetActive(_textureEntity.SpeedMove <= 0);
        _renderer.material.SetColor("_BaseColor", _textureEntity.BaseColor);
        _renderer.material.SetTexture("_BaseTexture", _textureEntity.BaseTexture);
        _renderer.material.SetTexture("_BaseAddingMask", _textureEntity.BaseAddedTexture.GetRandom(_worldPart.Xpos + _worldPart.Zpos));

        foreach (var b in _borders)
        {
            b.material.SetColor("_BaseColor", _textureEntity.BaseColor);
            b.material.SetTexture("_BaseTexture", _textureEntity.BaseTexture);
            b.material.SetTexture("_BaseAddingMask", _textureEntity.BaseAddedTexture.GetRandom(_worldPart.Xpos + _worldPart.Zpos));
        }

        GenerateBorders();
    }

    private void GenerateBorders()
    {
        List<SGEntity> sgs = new();
        foreach (var wp in _neigbors)
        {
            if (wp.Id <= _worldPart.Id)
                continue;

            var te = WorldViewer.Instance.GetTE(wp.Id);
            var x = _worldPart.Xpos - wp.Xpos;
            var z = _worldPart.Zpos - wp.Zpos;

            EnumTileDirect directionTile = Dict.GetDirectBySwift(x, z);

            sgs.Add(new SGEntity()
            {
                Id = te.Id,
                DirectionTexture = directionTile,
                Color = te.BaseColor,
            });
        }
        sgs = sgs.GroupBy(s => s.Id)
            .SelectMany(x =>
                WorldViewer.Instance.GetTE(x.Key).GetMask(x.Select(y => 
                    y.DirectionTexture).ToList().GetSummaryMask(), _worldPart.SwiftSeed))
            .OrderBy(x => x.Id).ToList();

        for (int layer = 1; layer < 9; layer++)
        {
            if (layer > sgs.Count)
            {
                _renderer.material.SetTexture($"_maskL{layer}", _textureBlackMask);
                _renderer.material.SetTexture($"_AddingMask{layer}", _textureBlackMask);
                _renderer.material.SetColor($"_colorL{layer}", Color.white);
                _renderer.material.SetFloat($"_rotL{layer}", 0);
            }
            else
            {
                _renderer.material.SetTexture($"_maskL{layer}", sgs[layer - 1].Mask);
                _renderer.material.SetTexture($"_AddingMask{layer}", sgs[layer - 1].AddingMask);
                _renderer.material.SetColor($"_colorL{layer}", sgs[layer - 1].Color);
                _renderer.material.SetFloat($"_rotL{layer}", sgs[layer - 1].Rotate);
            }
        }
    }

    private void CalcDecorations(int count = 0)
    {
        float distDecos = 10f / count;
        float baseSwift = distDecos / 2f - 10f / 2f;
        for (int x = 0; x < count; x++)
            for (int z = 0; z < count; z++)
            {
                var newDec = Instantiate(_prefabDecoration, _parentDecorations);
                newDec.transform.position = new Vector3(baseSwift + distDecos * x + transform.position.x, 0f,
                    baseSwift + distDecos * z + transform.position.z);
                newDec.Init(this);
                _decorations.Add(newDec);
            }
    }

    private void CleanDecorations()
    {
        var countTemp = _decorations.Count - 1;
        for (int i = countTemp; i >= 0; i--)
        {
            var deco = _decorations.ElementAt(0);
            Destroy(deco.gameObject);
            _decorations.Remove(deco);
        }
    }

    /// <summary>
    /// Method for digging
    /// </summary>
    public void ChangeTextureRandom()
    {
        var rndTxr = WorldViewer.Instance.Textures.GetRandom();
        _worldPart.SetNewId(rndTxr.Id);
    }

    public void VirtualDestroy()
    {
        gameObject.SetActive(false);
        _worldPart.ChangedId -= UpdateNeigbor;
        foreach (var tile in _neigbors)
        {
            tile.ChangedId -= UpdateNeigbor;
        }
        _neigbors.Clear();
        CleanDecorations();
    }

    private void OnDestroy()
    {
        _worldPart.ChangedId -= UpdateNeigbor;
        foreach (var tile in _neigbors)
        {
            tile.ChangedId -= UpdateNeigbor;
        }
    }

    public override int GetHashCode()
    {
        return Uid;
    }

    public override bool Equals(object obj)
    {
        if (obj is BasePlaneWorld other)
        {
            return Uid == other.Uid;
        }
        return false;
    }
}

public class SGEntity
{
    public EnumTileDirect DirectionTexture;
    public int Id;
    public Texture2D Mask;
    public Texture2D AddingMask;
    public Color Color;
    public float Rotate;
}