using UnityEngine;

[CreateAssetMenu(menuName = "States/WaitFinishInteractState", order = 1)]
public class WaitFinishInteractState : State
{
    private UsingByEntity _usingByEntity;

    protected override void Init()
    {
        var EM = Character.GetEntityMonobeh();
        _usingByEntity = EM.GetMyComponent<UsingByEntity>();
    }

    protected override void Run()
    {
        if (!_usingByEntity._isOpen)
        {
            IsFinished = true;
        }
    }

    public override void ExitState()
    {
        _usingByEntity.Close();
    }

    public override bool CheckRules(IStatesCharacter character)
    {
        var EM = character.GetEntityMonobeh();
        var ube = EM.GetMyComponent<UsingByEntity>();

        return ube._isOpen;
    }
}