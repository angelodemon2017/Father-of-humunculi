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

    private float _checkingRadius = 10f;
    private int _powerChanging = 0;

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
    }

    private void InitView()
    {
        _spriteRenderer.sprite = _basePlaneWorldParent.TextureEntity.GetDecor;
        _spriteRenderer.flipX = Random.Range(0, 10) > 5;
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

    private void CalcRadius()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, _checkingRadius);

        float dist = 0;
        float size = 0;

        foreach (var c in hitColliders)
        {
            var powDec = c.GetComponent<PowerDecoration>();
            if (powDec != null)
            {
                
            }
        }
    }
}