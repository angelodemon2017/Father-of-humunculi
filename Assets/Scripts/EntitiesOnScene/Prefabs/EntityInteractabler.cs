using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EntityInteractabler : PrefabByComponentData
{
    [SerializeField] private SphereCollider _sphereCollider;
    [SerializeField] private FSMController fSMController;

    private List<KeyHinter> _otherEnts = new();
    private List<KeyHinter> _forClear = new();

    public bool IsCanGo => _otherEnts.Count > 0 && GetNearKH.IsPressActionButton;
    public override string KeyComponent => typeof(EntityInteractabler).Name;
    public KeyHinter GetNearKH => _otherEnts.OrderBy(e => Vector3.Distance(e.transform.position, transform.position)).FirstOrDefault();

    private void Update()
    {
        CheckColliders();

        CheckInteract();
    }

    private void CheckInteract()
    {
        if (_otherEnts.Count > 0)
        {
            foreach (var ent in _otherEnts)
            {
                ent.IsNearest(ent == GetNearKH);
            }
        }
    }

    private void CheckColliders()
    {
        foreach (var h in _otherEnts)
        {
            if (h == null || !h.Entity.IsExist || !h.IsCanInteract)
            {
                _forClear.Add(h);
                continue;
            }
            if (Vector3.Distance(h.transform.position, transform.position) > Config.DistanceForShowHeyHint + _sphereCollider.radius)
            {
                h.Disconect();
                _forClear.Add(h);
            }
        }
        if (_forClear.Count > 0)
        {
            foreach (var c in _forClear)
            {
                _otherEnts.Remove(c);
            }
            _forClear.Clear();
        }
    }

    public void AddKeyHinter(KeyHinter keyHinter)
    {
        if (!_otherEnts.Contains(keyHinter) && keyHinter.IsCanInteract)
        {
            _otherEnts.Add(keyHinter);
        }
    }
}