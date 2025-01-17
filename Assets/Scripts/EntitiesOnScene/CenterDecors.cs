using UnityEngine;

public class CenterDecors : MonoBehaviour, IDictKey<Vector3Int>
{
    [SerializeField] private float _powerDecor = 1f;
    [SerializeField] private float _radius = 1f;

    public float PowerDecor => _powerDecor;
    public float Radius => _radius;

    private void OnEnable()
    {
    }

    internal void Init()
    {
        //        RunWave();
        WorldViewer.Instance.CashCentDecs.AddElement(this);
    }

    private void OnDisable()
    {
        WorldViewer.Instance.CashCentDecs.RemoveElement(this);
    }

    private void RunWave()
    {
        var pds = WorldViewer.Instance.CashPowerDecs.GetNeigsElements(transform.position);

        foreach (var pd in pds)
        {
            pd.CalcRadius();
        }
    }

    public Vector3Int GetKey()
    {
        return transform.position.GetChunkPosInt();
    }
}