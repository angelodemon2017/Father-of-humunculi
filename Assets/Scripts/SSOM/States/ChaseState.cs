using UnityEngine;

[CreateAssetMenu]
public class ChaseState : State
{
    [SerializeField] private float DistanceForTrigger;
    [SerializeField] private float DistanceForChasing;
    [SerializeField] private float DistanceForStopTrigger;
    [SerializeField] private float _chaseSpeed = 4;
    [SerializeField] private State _prepareAttackState;

    private UnityEngine.AI.NavMeshAgent _navMeshAgent;
    private StatusController _target;

    private float _currentDistance(IStatesCharacter chr) =>
        Vector3.Distance(chr.GetTransform().position, _target.transform.position);

    public override string DebugField => $"{_currentDistance(Character)}";

    protected override void Init()
    {
        _target = GetAvailableTarget(Character);
        _navMeshAgent = ((IMovableCharacter)Character).GetNavMeshAgent();
        _navMeshAgent.speed = _chaseSpeed;
    }

    protected override void Run()
    {
        var curDis = _currentDistance(Character);
        _navMeshAgent.SetDestination(_target.transform.position);

        if (curDis < DistanceForStopTrigger)
        {
            Character.SetState(_prepareAttackState);
        }

        if (curDis > DistanceForChasing)
        {
            IsFinished = true;
        }
    }

    public override bool CheckRules(IStatesCharacter character)
    {
        var avSC = GetAvailableTarget(character);

        return avSC != null;
    }

    public override void ExitState()
    {
        _navMeshAgent.SetDestination(Character.GetTransform().position);
    }

    private StatusController GetAvailableTarget(IStatesCharacter character)
    {
        var sc = character.GetStatusController();
        return sc.GetAvailableForAttack();
    }
}