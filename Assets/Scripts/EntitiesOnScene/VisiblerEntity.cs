using System;
using UnityEngine;

public class VisiblerEntity : MonoBehaviour
{
    [SerializeField] private EntityMonobeh _root;
    [SerializeField] private EnumFraction _whoIs;
    [SerializeField] private EnumControlInputPlayer _actionForInteract;
    [SerializeField] private MouseInterfaceInteraction _mouseInterfaceInteraction;

    private Transform _cashTransform;

    public bool IsExist => _root.IsExist;
    public EntityMonobeh Root => _root;
    public Vector3 Position => _cashTransform.position;
    public bool IsCanInteract => _root.IsExist && (_mouseInterfaceInteraction == null || _mouseInterfaceInteraction.CanInterAct);
    public bool IsCanInteractByPlayer => IsCanInteract && _actionForInteract.CheckAction();

    public Action<VisiblerEntity> OnVirtualDestroyer;

    private void Awake()
    {
        _cashTransform = transform;
    }

    public bool AvailableEntity(EnumFraction filter)
    {
        return IsExist && filter.HasFlag(_whoIs) && (_mouseInterfaceInteraction == null || _mouseInterfaceInteraction.CanInterAct);
    }

    public void Focusable()
    {
        if (_mouseInterfaceInteraction != null)
        {
            _mouseInterfaceInteraction.ShowTip();
        }
    }

    private void OnValidate()   
    {
        if (_root == null && transform.parent != null)
        {
            _root = transform.parent.GetComponent<EntityMonobeh>();
        }
    }

    private void OnDisable()
    {
        OnVirtualDestroyer?.Invoke(this);
    }
}