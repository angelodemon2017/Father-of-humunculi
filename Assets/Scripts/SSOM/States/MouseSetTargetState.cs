using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "States/MouseSetTargetState", order = 1)]
public class MouseSetTargetState : State
{
    [SerializeField] private float MoveSpeed;
    [SerializeField] private LayerMask _mask;
    [SerializeField] private float DistanceToTouch;
    private Vector3 _target;
    private NavMeshAgent _navMeshAgent;

    private RaycastHit hit;

    private EntityMonobeh _targetEM;

    protected override void Init()
    {
        var mii = TryGetMII();
        _targetEM = mii.RootMonobeh;
        _target = mii.transform.position;

        _navMeshAgent = ((IMovableCharacter)Character).GetNavMeshAgent();
        _navMeshAgent.SetDestination(_target);
    }

    protected override void Run()
    {
        if (_targetEM == null)
        {
            IsFinished = true;
        }

        var mii = TryGetMII();
        if (mii != null)
        {
            _targetEM = mii.RootMonobeh;
            _target = mii.transform.position;
            _navMeshAgent.SetDestination(_target);
        }

        if (Vector3.Distance(Character.GetTransform().position, _target) < DistanceToTouch)
        {
            if (_targetEM != null)
            {
                var myEM = Character.GetEntityMonobeh();

                var miicomp = _targetEM.PrefabsByComponents.GetComponent<MouseInterfaceInteraction>();
                miicomp.OnClick(myEM);
            }
            IsFinished = true;
        }
    }

    public override void ExitState()
    {
        _navMeshAgent.SetDestination(Character.GetTransform().position);
    }

    public override bool CheckRules(IStatesCharacter character)
    {
        return TryGetMII() != null;
    }

    private MouseInterfaceInteraction TryGetMII()
    {
        if (Input.GetMouseButtonDown(0) && !UIPlayerManager.ISCURSORUNDERUI)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100, _mask))
            {
                if (hit.transform.TryGetComponent(out MouseInterfaceInteraction mii) && mii.CanInterAct)
                {
                    return mii;
                }
            }
        }

        return null;
    }
}