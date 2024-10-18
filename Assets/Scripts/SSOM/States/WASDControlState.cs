using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "States/WASDControlState", order = 1)]
public class WASDControlState : State
{
    [SerializeField] private float _walkSpeed;

    private Vector3 _target = Vector3.zero;
    private NavMeshAgent _navMeshAgent;

    protected override void Init()
    {
        _navMeshAgent = ((IMovableCharacter)Character).GetNavMeshAgent();
        _navMeshAgent.speed = _walkSpeed;
    }

    protected override void Run()
    {
        _target = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
        {
            _target += Vector3.forward + Vector3.right;
        }
        if (Input.GetKey(KeyCode.A))
        {
            _target += Vector3.left + Vector3.forward;
        }
        if (Input.GetKey(KeyCode.S))
        {
            _target += Vector3.back + Vector3.left;
        }
        if (Input.GetKey(KeyCode.D))
        {
            _target += Vector3.right + Vector3.back;
        }

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
        return (Input.GetKey(KeyCode.W) ||
            Input.GetKey(KeyCode.A) ||
            Input.GetKey(KeyCode.S) ||
            Input.GetKey(KeyCode.D));
    }
}