using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "States/MouseControlState", order = 1)]
public class MouseControlState : State
{
    [SerializeField] private float MoveSpeed;
    [SerializeField] private LayerMask _mask;
    private Vector3 _target;
    private NavMeshAgent _navMeshAgent;

    private RaycastHit hit;

    protected override void Init()
    {
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
        if (Input.GetMouseButton(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100, _mask))
            {
                _target = hit.point;
                _navMeshAgent.SetDestination(_target);
            }
        }

        if (Vector3.Distance(Character.GetTransform().position, _target) < 0.1)
        {
            IsFinished = true;
        }
    }

    public override void ExitState()
    {
        _navMeshAgent.SetDestination(Character.GetTransform().position);
    }

    public override bool CheckRules(IStatesCharacter character)
    {
        return Input.GetMouseButtonDown(1);// && !UIPlayerManager.Instance.MouseOverUI;
    }
}