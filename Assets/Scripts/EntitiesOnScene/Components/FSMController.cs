using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FSMController : PrefabByComponentData, IStatesCharacter, IMovableCharacter
{
    public EntityMonobeh _entityMonobeh;
    [SerializeField] private State _startState;
    [SerializeField] private List<State> _availableState;
    [SerializeField] private NavMeshAgent _navMeshAgent;

    private Dictionary<string, State> _cashState = new();

    private State _currentState;
    private ComponentFSM _component;
    private Transform _transform;
    private Vector3 _lastPosition;
    private NavMeshSurfaceVolumeUpdater _navMeshSurfaceVolumeUpdater;

    public bool IsFinishedCurrentState() => _currentState.IsFinished;
    public Transform GetTransform() => _transform;
    public NavMeshAgent GetNavMeshAgent() => _navMeshAgent;
    public ComponentFSM ComponentData => _component;

    public override string KeyComponentData => typeof(ComponentFSM).Name;
    internal override ComponentData GetComponentData => new ComponentFSM(_startState.StateKey);
    public EntityMonobeh GetEntityMonobeh()
    {
        return _entityMonobeh;
    }

    public override void Init(ComponentData componentData, EntityInProcess entityInProcess = null)
    {
        _component = (ComponentFSM)componentData;
        _transform = _entityMonobeh.transform;
        if (_navMeshAgent == null)
        {
            _navMeshAgent = (NavMeshAgent)_transform.gameObject.AddComponent(typeof(NavMeshAgent));
            _navMeshAgent.angularSpeed = 0f;
        }
        _navMeshSurfaceVolumeUpdater = WorldViewer.Instance.GetUpdater();
        _navMeshSurfaceVolumeUpdater.Init(_navMeshAgent);

        SetState(GetState(_component.CurrentState));
    }

    public override void ExecuteCommand(EntityData entity, string command, string message, WorldData worldData)
    {
        switch (command)
        {
            case Dict.Commands.SetterState:
                SetStateKey(entity, message);
                break;
        }
    }

    private State GetState(string keyState)
    {
        if (_cashState.Count == 0)
        {
            _availableState.ForEach(x => _cashState.Add(x.StateKey, x));
        }

        return _cashState[keyState];
    }

    private void SetStateKey(EntityData entity, string message)
    {
        var cmpFSM = entity.Components.GetComponent<ComponentFSM>();
        if (cmpFSM != null)
        {
            if (cmpFSM.CurrentState != message)
            {
                cmpFSM.CurrentState = message;
                entity.UpdateEntity();
            }
        }
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

    internal override void VirtualDestroy()
    {
        WorldViewer.Instance.Remove(_navMeshSurfaceVolumeUpdater);
        _navMeshSurfaceVolumeUpdater = null;
    }

    public string TEST_CURRENT_STATE;

    public void SetState(State state, bool newState = false)
    {
        if (_currentState != null && _currentState.StateKey == state.StateKey)
        {
            return;
        }

        _currentState?.ExitState();
        if (_currentState != null)
        {
            Destroy(_currentState);
        }

        _currentState = newState ? state : Instantiate(state);
        _currentState.InitState(this);

        TEST_CURRENT_STATE = state.StateKey;
        _entityMonobeh.EntityInProcess.SendCommand(GetCommandSetState(state.StateKey));
    }

    private CommandData GetCommandSetState(string stateKey)
    {
        return new CommandData()
        {
            KeyComponent = KeyComponent,
            KeyCommand = Dict.Commands.SetterState,
            Message = stateKey,
        };
    }
}