using UnityEngine;

[CreateAssetMenu(menuName = "States/Item Use State", order = 1)]
public class ItemUseState : State
{
    private float _using = 0f;
    private ItemData _itemData;
    public override string DebugField => $"Использование {_itemConfig.Key}({(_itemConfig.TimeUse - _using).SimpleFormat()})";
    private ItemConfig _itemConfig;
    private string _tempKeyItem;

    internal void SetItem(ItemData itemData)
    {
        _itemData = itemData;
        _itemConfig = _itemData.ItemConfig;
        _tempKeyItem = _itemData.Id;
    }

    protected override void Init()
    {
        var bia = Character.GetEntityMonobeh().GetMyComponent<BaseInventoryAdapter>(0);
        var us = bia.ComponentInventory.UsingSlot;
        SetItem(bia.ComponentInventory.Items[us]);
    }

    protected override void Run()
    {
        _using += Time.deltaTime;

        if (_itemData == null || _tempKeyItem != _itemData.Id)
        {
            IsFinished = true;
            return;
        }

        if (_using >= _itemConfig.TimeUse)
        {
            _itemConfig.UseItem(_itemData, Character.GetEntityMonobeh().EntityInProcess.EntityData);

            IsFinished = true;
        }
    }

    public override void ExitState()
    {
        base.ExitState();
        var bia = Character.GetEntityMonobeh().GetMyComponent<BaseInventoryAdapter>(0);
        bia.ComponentInventory.UsingSlot = -1;
    }

    public override bool CheckRules(IStatesCharacter character)
    {
        var bia = character.GetEntityMonobeh().GetMyComponent<BaseInventoryAdapter>(0);
        var us = bia.ComponentInventory.UsingSlot;
        return //character.IsFinishedCurrentState() && 
            us >= 0;
    }
}