using UnityEngine;

[CreateAssetMenu(menuName = "States/IdleState", order = 1)]
public class IdleState : State
{
    [SerializeField] private float _timeIdle;
    [SerializeField] private float _timeRange;

    private float _timerIdle;

    public override string DebugField => $"считаю ворон...";

    protected override void Init()
    {
        _timerIdle = _timeIdle + SimpleExtensions.GetRandom(0, _timeRange);
    }

    protected override void Run()
    {
        if (_timerIdle > 0f)
        {
            _timerIdle -= Time.deltaTime;
        }

        if (_timerIdle < 0f)
        {
            IsFinished = true;
        }
    }

    public override bool CheckRules(IStatesCharacter character)
    {
        return character.IsFinishedCurrentState();
    }
}