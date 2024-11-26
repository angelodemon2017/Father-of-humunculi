using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "States/Key Set Target State", order = 1)]
public class KeySetTargetState : State
{
    [SerializeField] private float MoveSpeed;
    [SerializeField] private float DistanceToTouch;
    private Vector3 _target;
    private NavMeshAgent _navMeshAgent;

    private EntityMonobeh _targetEM;

    public override string DebugField => typeof(KeySetTargetState).Name;

    protected override void Init()
    {
        _target = _targetEM.transform.position;

        _navMeshAgent = ((IMovableCharacter)Character).GetNavMeshAgent();
        _navMeshAgent.SetDestination(_target);
    }

    protected override void Run()
    {
        if (_targetEM == null)
        {
            IsFinished = true;
        }

        if (Vector3.Distance(Character.GetTransform().position, _target) < DistanceToTouch)
        {
            if (_targetEM != null)
            {
                var myEM = Character.GetTransform().GetComponent<EntityMonobeh>();

                var miicomp = _targetEM.PrefabsByComponents.GetComponent<MouseInterfaceInteraction>();
                if (miicomp.CanInterAct)
                {
                    miicomp.OnClick(myEM);
                }
            }
            IsFinished = true;
        }
    }

    public void SetTarget(EntityMonobeh entity)
    {
        _targetEM = entity;
    }

    public override void ExitState()
    {
        _navMeshAgent.SetDestination(Character.GetTransform().position);
    }

    public override bool CheckRules(IStatesCharacter character)
    {
        return false;
    }
}