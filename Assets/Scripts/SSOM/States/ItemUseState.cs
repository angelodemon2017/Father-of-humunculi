using UnityEngine;

[CreateAssetMenu(menuName = "States/Item Use State", order = 1)]
public class ItemUseState : State
{
    private float _interactable = 0f;
    private int idSlotOfInventory;//?
    public override string DebugField => $"Использование {_itemConfig.Key}({(_itemConfig.TimeUse - _interactable).SimpleFormat()})";
    private ItemConfig _itemConfig;

    internal void SetMII(int idSlot)
    {
        idSlotOfInventory = idSlot;
    }

    protected override void Run()
    {
        _interactable += Time.deltaTime;

        if (_interactable > _itemConfig.TimeUse)
        {
            //TODO think about universal using items
            IsFinished = true;
        }
    }

    public override bool CheckRules(IStatesCharacter character)
    {
        return character.IsFinishedCurrentState();
    }
}