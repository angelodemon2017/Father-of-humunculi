using UnityEngine;

[CreateAssetMenu(menuName = "States/TouchTargetState", order = 1)]
public class TouchTargetState : State
{


    protected override void Init()
    {

    }

    public void SetTarget()
    {

    }

    protected override void Run()
    {

            IsFinished = true;
    }

    public override void ExitState()
    {

    }

    public override bool CheckRules(IStatesCharacter character)
    {
        return false;
    }
}