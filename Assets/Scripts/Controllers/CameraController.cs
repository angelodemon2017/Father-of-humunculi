using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance;
    [SerializeField] private float _intensive;
    [SerializeField] private Transform _currentTargetPoint;
    [SerializeField] private Transform _rootCamera;
    //    private Vector3 _diffVector;
    private Quaternion _directParalCamera;
//    private Quaternion _localRotation;

    [HideInInspector] public float CurAngl = 0f;

    public Vector3 FocusPosition => _currentTargetPoint != null ? _currentTargetPoint.position : Vector3.zero;
    public Vector3Int FocusTile => Vector3Int.RoundToInt(FocusPosition / Config.TileSize);
    public Quaternion DirectParalCamera => _directParalCamera;

    public static Action ChangedRot;
    public static Action<Vector3> ChangePosition;

    private void Awake()
    {
        Instance = this;

        RotateRoot(0f);
    }

    public void SetTarget(Transform newTarget)
    {
        _currentTargetPoint = newTarget;
    }

    private void Update()
    {
        if(EnumControlInputPlayer.TurnCameraLeft.CheckAction(true))
        {
            RotateRoot(45f);
        }
        if (EnumControlInputPlayer.TurnCameraRight.CheckAction(true))
        {
            RotateRoot(-45f);
        }

        _rootCamera.position = Vector3.Lerp(_rootCamera.position, FocusPosition, _intensive);
        ChangePosition?.Invoke(_rootCamera.position);
    }

    private void RotateRoot(float angl)
    {
        CurAngl += angl;
        _rootCamera.rotation = Quaternion.Euler(0f, CurAngl, 0f);
        Vector3 direction = _rootCamera.position - transform.position;
        _directParalCamera = Quaternion.LookRotation(direction);
        ChangedRot?.Invoke();
    }
}