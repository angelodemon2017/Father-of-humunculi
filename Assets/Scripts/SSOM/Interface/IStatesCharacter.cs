using UnityEngine;

public interface IStatesCharacter
{
    bool IsFinishedCurrentState();

    Transform GetTransform();

    StatusController GetStatusController();

    void PlayAnimation(EnumAnimations animation);

    void SetState(State state);

    bool CheckProp(EnumProps prop);

    void CreateObj(GameObject keepObj);
    void InitAttackZone(GameObject attackZone);
}