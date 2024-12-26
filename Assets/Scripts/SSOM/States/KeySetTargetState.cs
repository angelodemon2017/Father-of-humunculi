using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "States/Key Set Target State", order = 1)]
public class KeySetTargetState : State
{
    [SerializeField] private float MoveSpeed;
    [SerializeField] private float DistanceToTouch;
    [SerializeField] private InteractablerState _interactablerState;
    private Vector3 _target;
    private NavMeshAgent _navMeshAgent;
    private EntityMonobeh _targetEM;
    private MouseInterfaceInteraction _miicomp;

    public override string DebugField => $"иду к {_targetEM.GetTypeKey}";

    protected override void Init()
    {
        var visor = Character.GetEntityMonobeh().GetMyComponent<VisorComponent>();

        _targetEM = visor.GetNearByAction().Root;

        _target = _targetEM.transform.position;

        _navMeshAgent = ((IMovableCharacter)Character).GetNavMeshAgent();
        _navMeshAgent.SetDestination(_target);
        _miicomp = _targetEM.GetMyComponent<MouseInterfaceInteraction>();
    }

    protected override void Run()
    {
        if (_targetEM == null || !_targetEM.IsExist)
        {
            IsFinished = true;
            return;
        }

        if (Vector3.Distance(Character.GetTransform().position, _target) < DistanceToTouch)
        {
            if (_targetEM == null || !_miicomp.CanInterAct)
            {
                IsFinished = true;
            }
            else
            {
                var initingState = Instantiate(_interactablerState);
                initingState.SetMII(_miicomp);
                Character.SetState(initingState, true);
            }
        }
    }

    public override void ExitState()
    {
        if (_navMeshAgent != null)
        {
            _navMeshAgent.SetDestination(Character.GetTransform().position);
        }
    }

    public override bool CheckRules(IStatesCharacter character)
    {
        var visor = character.GetEntityMonobeh().GetMyComponent<VisorComponent>();

        return visor.AvailableEntity();
    }
}