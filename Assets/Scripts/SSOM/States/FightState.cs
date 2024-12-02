using UnityEngine;

[CreateAssetMenu(menuName = "States/Fight State", order = 1)]
public class FightState : State
{
    [SerializeField] private EnumFraction EntityFilter;
    [SerializeField] private float _timeAttack = 0.5f;
    [SerializeField] private float _attackDistance;

    private EntityMonobeh _targetEM;
    private float _timer;

    protected override void Init()
    {
        _timer = _timeAttack;
        var EM = Character.GetEntityMonobeh();
        var visor = EM.GetMyComponent<VisorComponent>();

        _targetEM = visor.GetNearEntity(EntityFilter).Root;

        //run attack animate
    }

    protected override void Run()
    {
        _timer -= Time.deltaTime;

        if (_timer <= 0f)
        {
            if (Vector3.Distance(Character.GetTransform().position, _targetEM.EntityInProcess.Position) < _attackDistance)
            {
                var interactCMP = _targetEM.GetMyComponent<MouseInterfaceInteraction>();
                interactCMP.AttackInteract(Character.GetEntityMonobeh());
            }
            IsFinished = true;
        }
    }

    public override bool CheckRules(IStatesCharacter character)
    {
        return true;
    }
}