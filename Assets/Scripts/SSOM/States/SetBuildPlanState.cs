using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "States/SetBuildPlanState", order = 1)]
public class SetBuildPlanState : State
{
    [SerializeField] private float MoveSpeed;
    [SerializeField] private LayerMask _mask;
    private Vector3 _target;
    private NavMeshAgent _navMeshAgent;

    private RaycastHit hit;

    public override string DebugField => $"моя ставить приблуду";

    protected override void Init()
    {
        UIPlayerManager.Instance.HideCursorBuild();

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 100, _mask))
        {
            _target = hit.point;
        }

        _navMeshAgent = ((IMovableCharacter)Character).GetNavMeshAgent();
        _navMeshAgent.SetDestination(_target);
        _navMeshAgent.speed = MoveSpeed;
    }

    protected override void Run()
    {
        if (Vector3.Distance(Character.GetTransform().position, _target) < 0.1)
        {
            UIPlayerManager.Instance.TrySetBuild(_target);
            IsFinished = true;
        }
    }

    public override void ExitState()
    {
        _navMeshAgent.SetDestination(Character.GetTransform().position);
    }

    public override bool CheckRules(IStatesCharacter character)
    {
        return Input.GetMouseButtonDown(0) && UIPlayerManager.Instance.IsReadySetBuild;
    }
}