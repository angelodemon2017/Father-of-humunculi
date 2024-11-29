using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VisorComponent : PrefabByComponentData
{
    [SerializeField] private List<VisiblerEntity> _visibleEntities = new();
    [SerializeField] private EnumFraction CanVisor;

    private Transform _cashTransform;
    private Vector3 _position => _cashTransform.position;

    private void Awake()
    {
        _cashTransform = transform;
    }

    public bool AvailableEntity()
    {
        if (_visibleEntities.Count == 0)
        {
            return false;
        }

        var res = _visibleEntities
            .Where(e => e.IsCanInteract)
            .OrderBy(e => Vector3.Distance(e.Position, _position))
            .FirstOrDefault();

        if (res != null)
        {
            res.Focusable();
            return res.IsCanInteractByPlayer;
        }
        else
        {
            return false;
        }
    }

    public VisiblerEntity GetNearByAction()
    {
        return _visibleEntities
            .Where(e => e.IsCanInteractByPlayer)
                .OrderBy(e => Vector3.Distance(e.Position, _position))
                .FirstOrDefault();
    }

    public bool AvailableEntity(EnumFraction entityFilter)
    {
        return _visibleEntities
            .Any(e => e.AvailableEntity(entityFilter));
    }

    public VisiblerEntity GetNearEntity(EnumFraction enumFraction)
    {
        return _visibleEntities
            .Where(e => e.AvailableEntity(enumFraction))
            .OrderBy(e => Vector3.Distance(e.Position, _position))
            .FirstOrDefault();
    }

    private void OnTriggerEnter(Collider other)
    {
        var ve = other.GetComponent<VisiblerEntity>();
        if (ve.AvailableEntity(CanVisor))
        {
            ve.OnVirtualDestroyer += DestroedVE;
            _visibleEntities.Add(ve);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var ve = other.GetComponent<VisiblerEntity>();
        if (_visibleEntities.Contains(ve))
        {
            _visibleEntities.Remove(ve);
        }
    }

    private void DestroedVE(VisiblerEntity visiblerEntity)
    {
        visiblerEntity.OnVirtualDestroyer -= DestroedVE;
        _visibleEntities.Remove(visiblerEntity);
    }
}