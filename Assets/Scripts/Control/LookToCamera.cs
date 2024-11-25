using UnityEngine;

public class LookToCamera : MonoBehaviour
{
    private Transform _thisTransform;
    [SerializeField] private bool _isRect;
    [SerializeField] private bool _isUpdate;

    private void Awake()
    {
        _thisTransform = transform;
    }

    private void Start()
    {
        UpdateLookRotation();
    }

    void Update()
    {
        if (_isUpdate)
        {
            UpdateLookRotation();
        }
    }

    private void UpdateLookRotation()
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