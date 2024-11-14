using UnityEngine;
using UnityEngine.AI;

public class FSMController : PrefabByComponentData, IStatesCharacter, IMovableCharacter
{
    public EntityMonobeh _entityMonobeh;
    [SerializeField] private State _startState;

    private State _currentState;
    private ComponentFSM _component;

    private NavMeshAgent _navMeshAgent;
    private Transform _transform;
    private Vector3 _lastPosition;

    public bool IsFinishedCurrentState() => _currentState.IsFinished;
    public Transform GetTransform() => _transform;
    public NavMeshAgent GetNavMeshAgent() => _navMeshAgent;

    public override string KeyComponent => typeof(FSMController).Name;
    public override string KeyComponentData => typeof(ComponentFSM).Name;
    internal override ComponentData GetComponentData => new ComponentFSM();
    public ComponentFSM ComponentData => _component;

    public override void Init(ComponentData componentData, EntityInProcess entityInProcess = null)
    {
        _component = (ComponentFSM)componentData;
        _transform = _entityMonobeh.transform;
        _navMeshAgent = (NavMeshAgent)_transform.gameObject.AddComponent(typeof(NavMeshAgent));
        _navMeshAgent.angularSpeed = 0f;
        SetState(_startState);
    }

    public void Init(Transform transform, State initState)
    {
        _transform = transform;
        _navMeshAgent = (NavMeshAgent)_transform.gameObject.AddComponent(typeof(NavMeshAgent));
        _navMeshAgent.angularSpeed = 0f;
        SetState(initState);
        _entityMonobeh = transform.GetComponent<EntityMonobeh>();
    }

    private void Update()
    {
        _currentState?.RunState();

        if (_lastPosition != _transform.position)
        {
            _lastPosition = _transform.position;
            _entityMonobeh.EntityInProcess.SendCommand(ComponentPosition.CommandUpdate(_lastPosition));
        }
    }

    public void SetState(State state, bool newState = false)
    {
        if (_currentState == state)
        {
            return;
        }

        _currentState?.ExitState();

        _currentState = newState ? state : Instantiate(state);
        _currentState.InitState(this);
    }

    public bool IsCurrentState(State checkedState)
    {
        return checkedState.DebugField == _currentState.DebugField;
    }
}