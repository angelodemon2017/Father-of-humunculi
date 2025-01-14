using System.Collections.Generic;
using UnityEngine;

public class PowerDecoration : MonoBehaviour
{
    [SerializeField] private BasePlaneWorld _basePlaneWorldParent;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private float SwiftRadius;
    [SerializeField] private int PowerCalcing = 2;
    [SerializeField] private bool NOTisDeco;
    [SerializeField] private AnimationCurve _scaleCurve;

    private int _powerChanging = 0;
    private float _localScale = 1f;

    private Vector3 _swiftScale => Vector3.one * Random.Range(1.5f, 2.5f);
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
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, Config.DistanceDecoration);

        float size = 1;

        foreach (var c in hitColliders)
        {
            if (c.TryGetComponent(out CenterDecors centerDecors) && c.gameObject.activeSelf)
            {
                var dist = Vector3.Distance(centerDecors.transform.position, transform.position);

                if (dist < centerDecors.Radius)
                {
                    size = 0;
                    break;
                }
                else
                {
                    var tempSize = _scaleCurve.Evaluate(dist - centerDecors.Radius) * centerDecors.PowerDecor;
                    if (size < tempSize)
                    {
                        size = tempSize;
                    }
                }
            }
        }

        _localScale = size;
        CalcSize();
    }
}