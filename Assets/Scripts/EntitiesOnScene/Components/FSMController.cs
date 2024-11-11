using UnityEngine;
using UnityEngine.AI;

public class FSMController : MonoBehaviour, IStatesCharacter, IMovableCharacter
{
    public EntityMonobeh _entityMonobeh;

    private State _currentState;

    private NavMeshAgent _navMeshAgent;
    private Transform _transform;
    private Vector3 _lastPosition;

    public bool IsFinishedCurrentState() => _currentState.IsFinished;
    public Transform GetTransform() => _transform;
    public NavMeshAgent GetNavMeshAgent() => _navMeshAgent;

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

    public void SetState(State state)
    {
        if (_currentState == state)
        {
            return;
        }

        _currentState?.ExitState();

        _currentState = Instantiate(state);
        _currentState.InitState(this);
    }

    public InteractableController GetStatusController()
    {
        throw new System.NotImplementedException();
    }
}