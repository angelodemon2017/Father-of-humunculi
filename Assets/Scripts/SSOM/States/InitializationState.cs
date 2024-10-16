using UnityEngine;

[CreateAssetMenu(menuName = "States/InitializationState", order = 1)]
public class InitializationState : State
{
    protected override void Init()
    {
        IsFinished = true;
    }

    public override bool CheckRules(IStatesCharacter character)
    {
        return true;
    }
}