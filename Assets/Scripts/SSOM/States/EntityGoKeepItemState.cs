using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "States/Entity Go Keep Item State", order = 1)]
public class EntityGoKeepItemState : State
{
    [SerializeField] private float MoveSpeed;
    [SerializeField] private float DistanceToTouch;
    [SerializeField] private EnumFraction EntityFilter;
    [SerializeField] private EnumHomuRole NeedHomuRole;

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

        if (Vector3.Distance(Character.GetTransform().position, _target) < DistanceToTouch)
        {
            if (_targetEM != null && _targetEM.IsExist)
            {
                var myEM = Character.GetEntityMonobeh();

                var miicomp = _targetEM.GetMyComponent<MouseInterfaceInteraction>();
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
        return RuleRole(character);
    }

    private bool RuleRole(IStatesCharacter character)
    {
        var EM = character.GetEntityMonobeh();
        var homuPresent = EM.GetMyComponent<HomuPresentPBCD>();
        if (homuPresent.Component.HomuRole != NeedHomuRole)
        {
            return false;
        }

        var visor = EM.GetMyComponent<VisorComponent>();

        return visor.AvailableEntity(EntityFilter);
    }
}