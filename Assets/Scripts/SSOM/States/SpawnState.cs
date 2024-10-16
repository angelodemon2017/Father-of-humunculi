using UnityEngine;

[CreateAssetMenu(menuName = "States/SpawnState", order = 1)]
public class SpawnState : State
{
    [SerializeField] private GameObject spawnObject;

    protected override void Init()
    {
        var az = Instantiate(spawnObject);
        az.transform.position = Character.GetTransform().position;
        IsFinished = true;
    }

    public override bool CheckRules(IStatesCharacter character)
    {
        return character.IsFinishedCurrentState();
    }
}