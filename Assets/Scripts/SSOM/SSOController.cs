using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;
using TMPro;

public class SSOController : MonoBehaviour, IStatesCharacter, IMovableCharacter
{
    [SerializeField] private TextMeshProUGUI _testText;
    [SerializeField] private NavMeshAgent _navMeshAgent;
    [SerializeField] private AnimationAdapter _animationAdapter;
    [SerializeField] private ColliderController _colliderController;
    [SerializeField] private InteractableController _interactableController;

    [SerializeField] private Transform _attackPoint;
    [SerializeField] private Transform _keepObjectPoint;
    [SerializeField] private State _startingState;
    [SerializeField] private State _currentState;

    [SerializeField] private string _debugField;

    private List<Property> _props = new();

    public bool IsFinishedCurrentState() => _currentState.IsFinished;

    public Transform GetTransform() => transform;
    public NavMeshAgent GetNavMeshAgent() => _navMeshAgent;
    public InteractableController GetStatusController() => _interactableController;

    private void Awake()
    {
        if (_colliderController)
        {
            _colliderController.ColliderAction += AddProp;
        }
        if (_animationAdapter)
        {
            _animationAdapter.triggerPropAction += AddProp;
        }
        SetState(_startingState);
        _interactableController.OnUpdateHP += UpdateTextLabel;
    }

    private void Update()
    {
        _currentState.RunState();
        _debugField = _currentState.DebugField;
        if (Input.GetKeyDown(KeyCode.Z))
        {

        }
    }

    private void LateUpdate()
    {
        ClearProps();
    }

    public void PlayAnimation(EnumAnimations animation)
    {
        _animationAdapter.PlayAnimationEvent(animation);
    }

    public void SetState(State state)
    {
        if (_currentState == state)
        {
            return;
        }

        _currentState?.ExitState();

        _currentState = Instantiate(state);
        _currentState.InitState(this);
        UpdateTextLabel();
    }

    private void UpdateTextLabel()
    {
        if (_testText != null)
        {
            var labelText = $"{_currentState.name}";

            if (!_interactableController.IsDeath)
            {
                labelText += $" HP:{_interactableController.CurrentHP}/{_interactableController.MaxHP}";
            }

            _testText.text = labelText;
        }
    }

    private void AddProp(EnumProps prop)
    {
        if (_props.Any(x => x.Prop == prop))
        {
            return;
        }

        _props.Add(new Property(prop));
    }

    public bool CheckProp(EnumProps prop)
    {
        var pr = _props.FirstOrDefault(x => x.Prop == prop);
        if (pr != null)
        {
            pr.Check = true;
        }
        return pr != null;
    }

    private void ClearProps()
    {
        List<Property> tempProps = new();

        foreach (var p in _props)
        {
            if (p.Check)
            {
                tempProps.Add(p);
            }
            else
            {
                p.Check = true;
            }
        }

        foreach (var p in tempProps)
        {
            _props.Remove(p);
        }
    }

    public void CreateObj(GameObject keepObj)
    {
        keepObj.transform.position = _keepObjectPoint.position;
    }

    public void InitAttackZone(GameObject attackZone)
    {
        attackZone.transform.SetParent(_attackPoint);
        attackZone.transform.position = _attackPoint.position;
    }
}