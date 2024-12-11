using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "States/WASDControlState", order = 1)]
public class WASDControlState : State
{
    [SerializeField] private float _walkSpeed;

    private Vector3 _target = Vector3.zero;
    private NavMeshAgent _navMeshAgent;

    public override string DebugField => $"Иду, куда укажут";

    protected override void Init()
    {
        _navMeshAgent = ((IMovableCharacter)Character).GetNavMeshAgent();
        _navMeshAgent.speed = _walkSpeed;
    }

    protected override void Run()
    {
        _target = Vector3.zero;

        if (EnumControlInputPlayer.MoveUp.CheckAction())
        {
            _target += Vector3.forward + Vector3.right;
        }
        if (EnumControlInputPlayer.MoveLeft.CheckAction())
        {
            _target += Vector3.left + Vector3.forward;
        }
        if (EnumControlInputPlayer.MoveDown.CheckAction())
        {
            _target += Vector3.back + Vector3.left;
        }
        if (EnumControlInputPlayer.MoveRight.CheckAction())
        {
            _target += Vector3.right + Vector3.back;
        }

        Quaternion rotation = Quaternion.Euler(Vector3.up * CameraController.Instance.CurAngl);
        _target = rotation * _target;

        var tempPos = Character.GetTransform().position;
        _target += tempPos;

        if (_target == tempPos)
        {
            IsFinished = true;
            _navMeshAgent.SetDestination(Character.GetTransform().position);
        }
        else
        {
            _navMeshAgent.SetDestination(_target);
        }
    }

    public override void ExitState()
    {
        _navMeshAgent.SetDestination(Character.GetTransform().position);
    }

    public override bool CheckRules(IStatesCharacter character)
    {
        return EnumControlInputPlayer.MoveDown.CheckAction() ||
            EnumControlInputPlayer.MoveLeft.CheckAction() ||
            EnumControlInputPlayer.MoveRight.CheckAction() ||
            EnumControlInputPlayer.MoveUp.CheckAction();
    }
}