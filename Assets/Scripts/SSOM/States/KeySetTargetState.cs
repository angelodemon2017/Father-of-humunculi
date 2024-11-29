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
        var visor = Character.GetEntityMonobeh().GetMyComponent<VisorComponent>();

        _targetEM = visor.GetNearByAction().Root;

        _target = _targetEM.transform.position;

        _navMeshAgent = ((IMovableCharacter)Character).GetNavMeshAgent();
        _navMeshAgent.SetDestination(_target);
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
            if (_targetEM != null)
            {
                var myEM = Character.GetEntityMonobeh();

                var miicomp = _targetEM.PrefabsByComponents.GetComponent<MouseInterfaceInteraction>();
                if (miicomp.CanInterAct)
                {
                    miicomp.OnClick(myEM);
                }
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
        var visor = character.GetEntityMonobeh().GetMyComponent<VisorComponent>();

        return visor.AvailableEntity();
    }
}