using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "States/EntityGoKeepItemState", order = 1)]
public class EntityGoKeepItemState : State
{
    [SerializeField] private float MoveSpeed;
    [SerializeField] private float DistanceToTouch;
    private Vector3 _target;
    private NavMeshAgent _navMeshAgent;

    private EntityMonobeh _targetEM;
    private HomuPresentPBCD _homuPresent;

    protected override void Init()
    {
        _target = _targetEM.transform.position;

        _navMeshAgent = ((IMovableCharacter)Character).GetNavMeshAgent();
        _navMeshAgent.SetDestination(_target);
    }

    public void SetTargetEM(EntityMonobeh targetEM, HomuPresentPBCD homuPresent)
    {
        _targetEM = targetEM;
        _homuPresent = homuPresent;
    }

    protected override void Run()
    {
        if (_targetEM == null)
        {
            IsFinished = true;
        }

/*        if (_targetEM != null && _targetEM.IsExist)
        {
            _target = _targetEM.transform.position;
            _navMeshAgent.SetDestination(_target);
        }/**/

        if (Vector3.Distance(Character.GetTransform().position, _target) < DistanceToTouch)
        {
            if (_targetEM != null && _targetEM.IsExist)
            {
                var myEM = Character.GetTransform().GetComponent<EntityMonobeh>();

                var miicomp = _targetEM.PrefabsByComponents.GetComponent<MouseInterfaceInteraction>();
                miicomp.OnClick(myEM);
            }
            IsFinished = true;
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
        return true;
    }
}