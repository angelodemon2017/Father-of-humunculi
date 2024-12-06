using UnityEngine;

public class AvailablerGroupRecipe : PrefabByComponentData
{
    [SerializeField] private GroupSO _groupSO;

    public GroupSO GetAvailableGroup => _groupSO;
    public string GetAvailableGroupName => _groupSO.GroupName;
}