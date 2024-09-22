using UnityEngine;

[CreateAssetMenu]
public class DeathState : State
{
    private UnityEngine.AI.NavMeshAgent _navMeshAgent;
    private Collider _bodyCollider;

    protected override void Init()
    {
        _navMeshAgent = ((IMovableCharacter)Character).GetNavMeshAgent();
        _navMeshAgent.enabled = false;
        _bodyCollider = Character.GetTransform().GetComponentInChildren<Collider>();
        _bodyCollider.enabled = false;
    }

    public override bool CheckRules(IStatesCharacter character)
    {
        var sc = character.GetStatusController();

        return sc.IsDeath;
    }
}