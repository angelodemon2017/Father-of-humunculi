using UnityEngine;

[CreateAssetMenu(menuName = "States/SpawnHumucleState", order = 1)]
public class SpawnHumucleState : State
{
    [SerializeField] private GameObject _humuncule;

    [SerializeField] private float _timeCD;
    [SerializeField] private KeyCode _keySpawn;

    private float _timerCD;

    protected override void Init()
    {
        _timerCD = _timeCD;
        var hum = Instantiate(_humuncule);
        Character.CreateObj(hum);
    }

    protected override void Run()
    {
        if (_timerCD > 0f)
        {
            _timerCD -= Time.deltaTime;
        }

        if (_timerCD < 0f)
        {
            IsFinished = true;
        }
    }

    public override bool CheckRules(IStatesCharacter character)
    {
        return Input.GetKey(_keySpawn);
    }
}