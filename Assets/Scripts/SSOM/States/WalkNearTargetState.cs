using UnityEngine;

[CreateAssetMenu(menuName = "States/Walk Near Target State", order = 1)]
public class WalkNearTargetState : State
{
    [SerializeField] private float _distanceForDone;
    [SerializeField] private float _distanceRandomPoint;
    [SerializeField] private float _speed = 3.5f;

    private UnityEngine.AI.NavMeshAgent _navMeshAgent;
    private Vector3 _target;
    private float _timeProblem = 0.6f;
    private float _timerProblem = 0f;
    private float _problemSwift = 0.05f;

    public override string DebugField => $"{_navMeshAgent.velocity.magnitude}";

    protected override void Init()
    {
        var fsmComp = Character.GetTransform().GetComponent<FSMController>();
        var targetEntity =
            GameProcess.Instance.GameWorld.GetEntityById(
                fsmComp.ComponentData.EntityTarget == 0 ?
                fsmComp._entityMonobeh.Id :
                fsmComp.ComponentData.EntityTarget);

        _timerProblem = _timeProblem;
        _navMeshAgent = ((IMovableCharacter)Character).GetNavMeshAgent();
        _navMeshAgent.speed = _speed;
        _target = SearchNewRandomTarget(targetEntity.Position, _distanceRandomPoint);
        _navMeshAgent.SetDestination(_target);
    }

    private Vector3 SearchNewRandomTarget(Vector3 centerPoint, float radius)
    {
        return new Vector3(
            Random.Range(centerPoint.x - radius, centerPoint.x + radius),
            centerPoint.y,
            Random.Range(centerPoint.z - radius, centerPoint.z + radius));
    }

    public override void ExitState()
    {
        _navMeshAgent.SetDestination(Character.GetTransform().position);
    }

    protected override void Run()
    {
        var distance = Vector3.Distance(Character.GetTransform().position, _target);

        if (distance < _distanceForDone)
        {
            IsFinished = true;
        }

        if (_navMeshAgent.velocity.magnitude < _problemSwift 
            && _timerProblem <= 0)
        {
            _timerProblem = _timeProblem;
        }
        else
        {
            _timerProblem -= Time.deltaTime;
            if (_timerProblem <= 0f)
            {
                IsFinished = true;
            }
        }
    }

    public override bool CheckRules(IStatesCharacter character)
    {
        return character.IsFinishedCurrentState();
    }
}