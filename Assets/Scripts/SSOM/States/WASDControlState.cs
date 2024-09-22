using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu]
public class WASDControlState : State
{
//    [SerializeField] private float _walkSpeed;

    private Vector3 _target = Vector3.zero;
//    private Vector3 direct;
    private NavMeshAgent _navMeshAgent;

    protected override void Init()
    {
        _navMeshAgent = ((IMovableCharacter)Character).GetNavMeshAgent();
    }

    protected override void Run()
    {
        _target = Vector3.zero;
        //        direct = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
        {
            _target += Vector3.forward;
        }
        if (Input.GetKey(KeyCode.A))
        {
            _target += Vector3.left;
        }
        if (Input.GetKey(KeyCode.S))
        {
            _target += Vector3.back;
        }
        if (Input.GetKey(KeyCode.D))
        {
            _target += Vector3.right;
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
//            Character.GetTransform().position += direct * _walkSpeed;
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
            Input.GetKey(KeyCode.D));// &&
//            character.IsFinishedCurrentState();
    }
}