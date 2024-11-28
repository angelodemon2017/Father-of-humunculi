using UnityEngine;

[CreateAssetMenu(menuName = "States/Switch State", order = 1)]
public class SwitcherState : State
{
    protected override void Init()
    {
        IsFinished = true;
    }

    public override bool CheckRules(IStatesCharacter character)
    {
        return character.IsFinishedCurrentState();
    }
}