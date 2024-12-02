using UnityEngine;

[CreateAssetMenu(menuName = "States/Death State", order = 1)]
public class DeathState : State
{
    [SerializeField] private float _timeDeath = 1f;

    private float _timer;

    protected override void Init()
    {
        _timer = _timeDeath;

        //spawn items
    }

    protected override void Run()
    {
        _timer -= Time.deltaTime;
        if (_timer <= 0f)
        {
            IsFinished = true;
        }
    }

    public override bool CheckRules(IStatesCharacter character)
    {
        return IsDeath(character);
    }

    private bool IsDeath(IStatesCharacter character)
    {
        var cmp = character.GetEntityMonobeh().EntityInProcess.EntityData.Components.GetComponent<ComponentHPData>();

        return cmp.IsDeath;
    }
}