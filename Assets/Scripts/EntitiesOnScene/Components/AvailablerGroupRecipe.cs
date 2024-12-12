using UnityEngine;
using static OptimazeExtensions;

public class AvailablerGroupRecipe : PrefabByComponentData
{
    public override int KeyType => TypeCache<AvailablerGroupRecipe>.IdType;
    [SerializeField] private GroupSO _groupSO;

    public GroupSO GetAvailableGroup => _groupSO;
    public string GetAvailableGroupName => _groupSO.GroupName;
}