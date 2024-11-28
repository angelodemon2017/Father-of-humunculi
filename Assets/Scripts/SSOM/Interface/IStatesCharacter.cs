using UnityEngine;

public interface IStatesCharacter
{
    bool IsFinishedCurrentState();

    Transform GetTransform();

    EntityMonobeh GetEntityMonobeh();

    void SetState(State state, bool newState = false);

//    bool CheckProp(EnumProps prop);

//    void InitAttackZone(GameObject attackZone);
}