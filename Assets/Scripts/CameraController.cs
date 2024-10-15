using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance;
    [SerializeField] private Transform _targetPoint;
    [SerializeField] private float _intensive;
    private Transform _currentTargetPoint;
    private Vector3 _diffVector;
    private Quaternion _directParalCamera;

    public Vector3 FocusPosition => _targetPoint.position;
    public Quaternion DirectParalCamera => _directParalCamera;

    private void Awake()
    {
        Instance = this;

        _diffVector = transform.position - _targetPoint.position;
        _currentTargetPoint = _targetPoint;

        Vector3 direction = _currentTargetPoint.position - transform.position;
        _directParalCamera = Quaternion.LookRotation(direction);
    }

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, _currentTargetPoint.position + _diffVector, _intensive);
    }
}