using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu]
public class TextureEntity : ScriptableObject
{
    [SerializeField] private int _id;
    [SerializeField] private Texture2D _baseTexture;
    [SerializeField] private List<Texture2D> _baseAddedTexture;
    [SerializeField] private Color _baseColor;
    [SerializeField] private Material _sgmaterial;
    [SerializeField] private List<TextureCombine> _maskCombine;
    [SerializeField] private float _speedMove = 1f;

    public int Id => _id;
    public Texture2D BaseTexture => _baseTexture;
    public List<Texture2D> BaseAddedTexture => _baseAddedTexture;
    public Color BaseColor => _baseColor;
    public float SpeedMove => _speedMove;

    public List<SGEntity> GetMask(EnumTileDirect summaryDirect, int swiftSeed)
    {
        if (summaryDirect == EnumTileDirect.none)
        {
            return new();
        }

        List<SGEntity> sges = new();

        while (summaryDirect != EnumTileDirect.none)
        {
            var sgeTemp = _maskCombine.FirstOrDefault(x => x.Contain(summaryDirect)).GetEntity(summaryDirect, swiftSeed);

            sgeTemp.Item1.Id = _id;
            sgeTemp.Item1.AddingMask = _baseAddedTexture.GetRandom(swiftSeed);
            sgeTemp.Item1.Color = _baseColor;

            sges.Add(sgeTemp.Item1);

            summaryDirect = summaryDirect & ~sgeTemp.Item2;
        }

        return sges;
    }
}

[System.Serializable]
public class TextureCombine
{
    public List<Texture2D> Masks;
    public List<CombineEnumRotate> enumDirects;

    public (SGEntity, EnumTileDirect) GetEntity(EnumTileDirect summaryDirect, int swiftSeed = 0)
    {
        var cer = GetCombineRotate(summaryDirect);

        return (new SGEntity()
        {
            Mask = Masks.GetRandom(swiftSeed + (int)cer.TileDirect),
            DirectionTexture = cer.EnumDirect,
            Rotate = (int)cer.TileDirect,
        },
        cer.ExceptDirects);
    }

    public CombineEnumRotate GetCombineRotate(EnumTileDirect summaryDirect)
    {
        return enumDirects.FirstOrDefault(x => (x.EnumDirect & summaryDirect) == x.EnumDirect);
    }

    public bool Contain(EnumTileDirect summaryDirect)
    {
        return enumDirects.Any(x => (x.EnumDirect & summaryDirect) == x.EnumDirect);
    }

    [System.Serializable]
    public class CombineEnumRotate
    {
        public EnumTileDirect EnumDirect;
        public EnumShaderMaskDirect TileDirect;
        public EnumTileDirect ExceptDirects;
    }
}