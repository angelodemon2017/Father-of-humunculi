using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "States/Go To Entity By Visor State", order = 1)]
public class GoToEntityByVisor : State
{
    [SerializeField] private float MoveSpeed;
    [SerializeField] private float DistanceToTouch;
    [SerializeField] private EnumFraction EntityFilter;
    [SerializeField] private State _stateSuccesTarget;

    private Vector3 _target;
    private NavMeshAgent _navMeshAgent;
    private EntityMonobeh _targetEM;

    protected override void Init()
    {
        var EM = Character.GetEntityMonobeh();
        var visor = EM.GetMyComponent<VisorComponent>();

        _targetEM = visor.GetNearEntity(EntityFilter).Root;

        _target = _targetEM.transform.position;

        _navMeshAgent = ((IMovableCharacter)Character).GetNavMeshAgent();
        _navMeshAgent.SetDestination(_target);
    }

    protected override void Run()
    {
        if (_targetEM == null || IsFinished)
        {
            IsFinished = true;
            return;
        }

        var myTempPosit = Character.GetTransform().position;
        var distToPlace = Vector3.Distance(myTempPosit, _target);
        var distToTarget = Vector3.Distance(myTempPosit, _targetEM.transform.position);

        if (distToPlace < DistanceToTouch || distToTarget < DistanceToTouch)
        {
            if (distToTarget > DistanceToTouch)
            {
                _target = _targetEM.transform.position;
                _navMeshAgent.SetDestination(_target);
            }
            else
            {
                if (_targetEM != null && _targetEM.IsExist)
                {
                    Character.SetState(_stateSuccesTarget);
                }
                IsFinished = true;
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
        return RuleRole(character);
    }

    private bool RuleRole(IStatesCharacter character)
    {
        var EM = character.GetEntityMonobeh();
        var visor = EM.GetMyComponent<VisorComponent>();
        return visor.AvailableEntity(EntityFilter);
    }
}