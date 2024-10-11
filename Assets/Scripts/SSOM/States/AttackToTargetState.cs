using UnityEngine;

[CreateAssetMenu(menuName = "States/AttackToTargetState", order = 1)]
public class AttackToTargetState : State
{
    [SerializeField] private float _timeAttack;
    [SerializeField] private int Power;

    private float _timerAttack;
    private InteractableController _target;

    protected override void Init()
    {
        _target = GetAvailableTarget(Character);
        _timerAttack = _timeAttack;
        _target.TakeDamage(Power);
    }

    protected override void Run()
    {
        if (_timerAttack > 0f)
        {
            _timerAttack -= Time.deltaTime;
        }

        if (_timerAttack < 0f)
        {
            IsFinished = true;
        }
    }

    public override bool CheckRules(IStatesCharacter character)
    {
        _target = GetAvailableTarget(character);
        return character.IsFinishedCurrentState() && _target != null;
    }

    private InteractableController GetAvailableTarget(IStatesCharacter character)
    {
        var sc = character.GetStatusController();
        return sc.GetAvailableForAttack();
    }
}