using UnityEngine;
using UnityEngine.AI;

public class FSMController : MonoBehaviour, IStatesCharacter, IMovableCharacter
{
    private State _currentState;

    private NavMeshAgent _navMeshAgent;
    private Transform _transform;

    public bool IsFinishedCurrentState() => _currentState.IsFinished;
    public Transform GetTransform() => _transform;
    public NavMeshAgent GetNavMeshAgent() => _navMeshAgent;

    public void Init(Transform transform, State initState)
    {
        _transform = transform;
        _navMeshAgent = (NavMeshAgent)_transform.gameObject.AddComponent(typeof(NavMeshAgent));
        _navMeshAgent.angularSpeed = 0f;
        SetState(initState);
    }

    private void Update()
    {
        _currentState?.RunState();
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