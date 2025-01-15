using System.Collections.Generic;
using UnityEngine;

public class PowerDecoration : MonoBehaviour, IObjectOfPool, IDictKey<Vector3Int>
{
    [SerializeField] private BasePlaneWorld _basePlaneWorldParent;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private float SwiftRadius;
    [SerializeField] private int PowerCalcing = 2;
    [SerializeField] private bool NOTisDeco;
    [SerializeField] private AnimationCurve _scaleCurve;

    private int _powerChanging = 0;
    private float _localScale = 1f;

    private Vector3 _swiftScale => Vector3.one * Random.Range(0.2f, 0.5f);
    public float Radius => transform.localScale.x * SwiftRadius;
    public int PowerChanging => _powerChanging;

    private void Start()
    {
//        InitChange(PowerCalcing);
    }

    public void Init(BasePlaneWorld basePlaneWorld)
    {
        _basePlaneWorldParent = basePlaneWorld;
        InitView();
        CalcRadius();
        WorldViewer.Instance.CashPowerDecs.AddElement(this);
    }

    private void InitView()
    {
        _spriteRenderer.sprite = _basePlaneWorldParent.TextureEntity.GetDecor;
        _spriteRenderer.flipX = Random.Range(0, 10) > 5;
    }

    private void CalcSize()
    {
        transform.localScale = _swiftScale * _localScale;
    }

    private void InitChange(int power)
    {
        var changingDecos = CalcWave(power);

        foreach (var n in changingDecos)
        {
            n.EndCalcing();
        }
    }

    public void EndCalcing()
    {
        _powerChanging = 0;
    }

    private HashSet<PowerDecoration> GetNeigbors()
    {
        return new HashSet<PowerDecoration>();
    }

    private HashSet<PowerDecoration> CalcWave(int powerChang)
    {
        _powerChanging = powerChang;
        CalcRadius();

        var neigs = GetNeigbors();
        var changingDecos = new HashSet<PowerDecoration>();
        foreach (var n in neigs)
        {
            if (n.PowerChanging < _powerChanging)
            {
                changingDecos.Add(n);
                n.CalcWave(_powerChanging - 1);
            }
        }
        return changingDecos;
    }

    internal void CalcRadius()
    {
        float size = 1;
        var cds = WorldViewer.Instance.CashCentDecs.GetNeigsElements(transform.position);

        foreach (var cd in cds)
        {
            var dist = Vector3.Distance(cd.transform.position, transform.position);
            if (dist < cd.Radius)
            {
                size = 0;
                break;
            }
            else
            {
                var tempSize = _scaleCurve.Evaluate(dist - cd.Radius) * cd.PowerDecor;
                if (size < tempSize)
                {
                    size = tempSize;
                }
            }
        }

        _localScale = size;
        CalcSize();
    }

    private void OnDestroy()
    {
    }

    public void VirtualCreate()
    {

    }

    public void VirtualDestroy()
    {
        WorldViewer.Instance.CashPowerDecs.RemoveElement(this);
    }

    public Vector3Int GetKey()
    {
        return transform.position.GetChunkPosInt();
    }
}