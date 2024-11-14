using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EntityInteractabler : MonoBehaviour//PrefabByComponentData
{
    [SerializeField] private SphereCollider _sphereCollider;
    [SerializeField] private FSMController fSMController;
    [SerializeField] private KeySetTargetState _stateSetTarget;

    private List<KeyHinter> _otherEnts = new();
    private List<KeyHinter> _forClear = new();

    private void Update()
    {
        CheckColliders();

        CheckInteract();
    }

    private void CheckInteract()
    {
        if (_otherEnts.Count > 0 && !fSMController.IsCurrentState(_stateSetTarget))
        {
            var kh = _otherEnts.OrderBy(e => Vector3.Distance(e.transform.position, transform.position)).FirstOrDefault();
            if (kh.IsPressActionButton)
            {
                var tempState = Instantiate(_stateSetTarget);

                tempState.SetTarget(kh.Entity);

                fSMController.SetState(tempState, true);
            }
        }
    }

    private void CheckColliders()
    {
        foreach (var h in _otherEnts)
        {
            if (h == null)
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
        if (!_otherEnts.Contains(keyHinter))
        {
            _otherEnts.Add(keyHinter);
        }
    }
}