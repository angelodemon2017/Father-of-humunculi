using UnityEngine;

public class CenterDecors : MonoBehaviour
{
    [SerializeField] private float _powerDecor = 1f;
    [SerializeField] private float _radius = 1f;

    public float PowerDecor => _powerDecor;
    public float Radius => _radius;

    private void OnEnable()
    {
        RunWave();
    }

    private void RunWave()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, Config.DistanceDecoration);
        foreach (var hc in hitColliders)
        {
            if (hc.TryGetComponent(out PowerDecoration powerDecoration))
            {
//                Debug.Log($"RunWave PowerDecoration.count:{hitColliders.Length}");
                powerDecoration.CalcRadius();
            }
        }
    }
}