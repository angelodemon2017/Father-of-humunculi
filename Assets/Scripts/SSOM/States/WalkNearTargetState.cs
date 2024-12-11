using UnityEngine;

[CreateAssetMenu(menuName = "States/Walk Near Target State", order = 1)]
public class WalkNearTargetState : State
{
    [SerializeField] private float _distanceForDone;
    [SerializeField] private float _distanceRandomPoint;
    [SerializeField] private float _speed = 3.5f;

    private UnityEngine.AI.NavMeshAgent _navMeshAgent;
    private Vector3 _target;
    private float _timeProblem = 2f;
    private float _timerProblem = 0f;
    private float _problemSwift = 0.05f;

    private FSMController _fSMController;

    public override string DebugField => $"{_navMeshAgent.velocity.magnitude}";

    protected override void Init()
    {
        _fSMController = Character.GetEntityMonobeh().GetMyComponent<FSMController>();
        var targetEntity = MainFocus(_fSMController);

        _timerProblem = _timeProblem;
        _navMeshAgent = ((IMovableCharacter)Character).GetNavMeshAgent();
        _navMeshAgent.speed = _speed;
        _target = SearchNewRandomTarget(targetEntity, _distanceRandomPoint);
        _navMeshAgent.SetDestination(_target);
    }

    public static Vector3 MainFocus(FSMController fSMController)
    {
        return fSMController.ComponentData.EntityTarget == -1 ?
               new Vector3(fSMController.ComponentData.xPosFocus, 0f, fSMController.ComponentData.zPosFocus) :
               GameProcess.Instance.GameWorld.GetEntityById(fSMController.ComponentData.EntityTarget).Position;
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
        if (_navMeshAgent != null)
        {
            _navMeshAgent.SetDestination(Character.GetTransform().position);
        }
    }

    protected override void Run()
    {
        var distance = Vector3.Distance(Character.GetTransform().position, _target);

        if (distance < _distanceForDone)
        {
            var CheckPoint = MainFocus(_fSMController);
            var controlDistance = Vector3.Distance(CheckPoint, Character.GetTransform().position);
            if (controlDistance < _distanceForDone)
            {
                IsFinished = true;
                return;
            }

            _target = SearchNewRandomTarget(CheckPoint, _distanceRandomPoint);
            _navMeshAgent.SetDestination(_target);
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
//                Debug.Log($"Time Problem in WalkNearTargetState");
                IsFinished = true;
            }
        }
    }

    public override bool CheckRules(IStatesCharacter character)
    {
        var fSMController = character.GetEntityMonobeh().GetMyComponent<FSMController>();
        var targetEntity = MainFocus(fSMController);

        return character.IsFinishedCurrentState() && Vector3.Distance(targetEntity, character.GetTransform().position) > _distanceRandomPoint * 2;
    }
}