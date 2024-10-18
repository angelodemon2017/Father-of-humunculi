using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance;
//    [SerializeField] private Transform _targetPoint;
    [SerializeField] private float _intensive;
    [SerializeField] private Transform _currentTargetPoint;
    private Vector3 _diffVector;
    private Quaternion _directParalCamera;

    public Vector3 FocusPosition => _currentTargetPoint != null ? _currentTargetPoint.position : Vector3.zero;
    public Quaternion DirectParalCamera => _directParalCamera;

    private void Awake()
    {
        Instance = this;

        _diffVector = transform.position - Vector3.zero;
//        SetTarget(_targetPoint);

        Vector3 direction = Vector3.zero - transform.position;
        _directParalCamera = Quaternion.LookRotation(direction);
    }

    public void SetTarget(Transform newTarget)
    {
        _currentTargetPoint = newTarget;
    }

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, _currentTargetPoint.position + _diffVector, _intensive);
    }
}