using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemAction/RandomAction", order = 1)]
public class ItemActionRandomAction : ItemActionConfig
{
    public List<ItemActionConfig> _listAvailableActions = new();

    private int _randomPick = 0;

    public override bool AvailableUseItem(ItemData itemData, EntityData entityData)
    {
        _randomPick = Random.Range(0, _listAvailableActions.Count);
        return _listAvailableActions[_randomPick].AvailableUseItem(itemData, entityData);
    }

    public override void ApplyItem(ItemData itemData, EntityData entityData)
    {
        _listAvailableActions[_randomPick].ApplyItem(itemData, entityData);
    }
}