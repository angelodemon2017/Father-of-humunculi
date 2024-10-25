using UnityEngine;

public class LookToCamera : MonoBehaviour
{
    private Transform _thisTransform;
    [SerializeField] private bool _isRect;

    private void Awake()
    {
        _thisTransform = transform;
    }

    void Update()
    {
        if (CameraController.Instance != null)
        {
            if (_isRect)
            {
                _thisTransform.rotation = CameraController.Instance.DirectParalCamera;
            }
            else
            {
                _thisTransform.localRotation = CameraController.Instance.DirectParalCamera;
            }
        }
    }
}