using UnityEngine;

[CreateAssetMenu(menuName = "States/Interactabler State", order = 1)]
public class InteractablerState : State
{
    private float _interactable = 0f;
    private MouseInterfaceInteraction _miicomp;
    public override string DebugField => $"взаимодейств.({(_miicomp.TimeInteract - _interactable).ToString("#.##")})";

    internal void SetMII(MouseInterfaceInteraction mii)
    {
        _miicomp = mii;
    }

    protected override void Run()
    {
        _interactable += Time.deltaTime;

        if (_interactable > _miicomp.TimeInteract)
        {
            var myEM = Character.GetEntityMonobeh();

            _miicomp.OnClick(myEM);
            IsFinished = true;
        }
    }

    public override bool CheckRules(IStatesCharacter character)
    {
        return character.IsFinishedCurrentState();
    }
}