using UnityEngine;

public interface IStatesCharacter
{
    bool IsFinishedCurrentState();

    Transform GetTransform();

//    InteractableController GetStatusController();

//    void PlayAnimation(EnumAnimations animation);

    void SetState(State state);

//    bool CheckProp(EnumProps prop);

//    void CreateObj(GameObject keepObj);
//    void InitAttackZone(GameObject attackZone);
}