using UnityEngine;

[CreateAssetMenu(menuName = "States/Fight State", order = 1)]
public class FightState : State
{
    protected override void Init()
    {

    }

    protected override void Run()
    {

    }

    public override bool CheckRules(IStatesCharacter character)
    {
        return character.IsFinishedCurrentState();
    }
}