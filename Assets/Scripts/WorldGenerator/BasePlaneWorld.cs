using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BasePlaneWorld : MonoBehaviour
{
    [SerializeField] private Renderer _renderer;
    private WorldTile _worldPart;

    private List<WorldTile> _neigbors = new();
    [SerializeField] private Texture2D _textureBlackMask;

    public void Init(WorldTile worldPart, List<WorldTile> neigbors)
    {
        _worldPart = worldPart;
        foreach (var wp in neigbors)
        {
            _neigbors.Add(wp);
            wp.ChangedId += UpdateNeigbor;
        }
        transform.position = new Vector3(_worldPart.Xpos * 5 - WorldGenerator.Instance.WorldSwift.x, 0f, _worldPart.Zpos * 5 - WorldGenerator.Instance.WorldSwift.y);
        UpdatePart();
    }

    private void UpdateNeigbor(int id)
    {
        UpdatePart();
    }

    public void UpdatePart()
    {
        var _textureEntity = WorldGenerator.Instance.GetTE(_worldPart.Id);

        _renderer.material.SetColor("_BaseColor", _textureEntity.BaseColor);
        _renderer.material.SetTexture("_BaseTexture", _textureEntity.BaseTexture);

        GenerateBorders();
    }

    private void GenerateBorders()
    {
        List<SGEntity> sgs = new();
        foreach (var wp in _neigbors)
        {
            if (wp.Id <= _worldPart.Id)
                continue;

            var te = WorldGenerator.Instance.GetTE(wp.Id);
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
                WorldGenerator.Instance.GetTE(x.Key).GetMask(x.Select(y => 
                    y.DirectionTexture).ToList().GetSummaryMask()))
            .OrderBy(x => x.Id).ToList();

        for (int layer = 1; layer < 9; layer++)
        {
            if (layer > sgs.Count)
            {
                _renderer.material.SetTexture($"_maskL{layer}", _textureBlackMask);
                _renderer.material.SetColor($"_colorL{layer}", Color.white);
                _renderer.material.SetFloat($"_rotL{layer}", 0);
            }
            else
            {
                _renderer.material.SetTexture($"_maskL{layer}", sgs[layer - 1].Mask);
                _renderer.material.SetColor($"_colorL{layer}", sgs[layer - 1].Color);
                _renderer.material.SetFloat($"_rotL{layer}", sgs[layer - 1].Rotate);
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            GenerateBorders();
        }
    }
}

public class SGEntity
{
    public EnumTileDirect DirectionTexture;
    public int Id;
    public Texture2D Mask;
    public Color Color;
    public float Rotate;

    public SGEntity()
    { }

    public SGEntity(int id, Texture2D mask, EnumTileDirect directionTexture, int rot)
    {
        Id = id;
        Mask = mask;
        DirectionTexture = directionTexture;
        Rotate = rot;
    }
}