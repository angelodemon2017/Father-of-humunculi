using UnityEngine;
using UnityEngine.AI;

public class FSMController : PrefabByComponentData, IStatesCharacter, IMovableCharacter
{
    public EntityMonobeh _entityMonobeh;
    [SerializeField] private State _startState;
    [SerializeField] private NavMeshAgent _navMeshAgent;

    private State _currentState;
    private ComponentFSM _component;
    private Transform _transform;
    private Vector3 _lastPosition;
    private NavMeshSurfaceVolumeUpdater _navMeshSurfaceVolumeUpdater;

    public State GetCurrentState => _currentState;
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
        if (_navMeshAgent == null)
        {
            _navMeshAgent = (NavMeshAgent)_transform.gameObject.AddComponent(typeof(NavMeshAgent));
            _navMeshAgent.angularSpeed = 0f;
        }
        _navMeshSurfaceVolumeUpdater = WorldViewer.Instance.GetUpdater();
        _navMeshSurfaceVolumeUpdater.Init(_navMeshAgent);

        SetState(_startState);
    }

    public override void ExecuteCommand(EntityData entity, string command, string message, WorldData worldData)
    {
        switch (command)
        {
            case Dict.Commands.SetterState:
                SetStateKey(entity, message, worldData);
                break;
        }
    }

    private void SetStateKey(EntityData entity, string message, WorldData worldData)
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

    public void SetState(State state, bool newState = false)
    {
        if (_currentState == state)
        {
            return;
        }

        _currentState?.ExitState();

        _currentState = newState ? state : Instantiate(state);
        _currentState.InitState(this);

        _entityMonobeh.EntityInProcess.SendCommand(GetCommandSetState(state.StateKey));
    }

    public bool IsCurrentState(State checkedState)
    {
        return checkedState.DebugField == _currentState.DebugField;
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