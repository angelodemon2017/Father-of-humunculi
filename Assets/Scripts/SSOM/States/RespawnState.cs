using UnityEngine;

[CreateAssetMenu(menuName = "States/RespawnState", order = 1)]
public class RespawnState : State
{
    private UnityEngine.AI.NavMeshAgent _navMeshAgent;
    private Collider _bodyCollider;

    protected override void Init()
    {
        _navMeshAgent = ((IMovableCharacter)Character).GetNavMeshAgent();
        _navMeshAgent.enabled = true;

        _bodyCollider = Character.GetTransform().GetComponentInChildren<Collider>();
        _bodyCollider.enabled = true;

        var sc = Character.GetStatusController();
        sc.Respawn();

        IsFinished = true;
    }

    public override bool CheckRules(IStatesCharacter character)
    {
        var sc = character.GetStatusController();

        return sc.IsDeath && Input.GetKey(KeyCode.R);
    }
}