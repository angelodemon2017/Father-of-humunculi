using UnityEngine;

[CreateAssetMenu(menuName = "States/WaitFinishInteractState", order = 1)]
public class WaitFinishInteractState : State
{
    private State _nextState;
    private GameObject _panel;

    protected override void Run()
    {
        if (!_panel.activeSelf)
        {
            Character.SetState(_nextState, true);
        }
    }

    public void SetSpectator(GameObject panel, State nextState)
    {
        _panel = panel;
        _nextState = nextState;
    }

    public override bool CheckRules(IStatesCharacter character)
    {
        return true;
    }
}