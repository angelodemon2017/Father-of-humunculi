using UnityEngine;

[CreateAssetMenu(menuName = "Quests/Items Quest", order = 1)]
public class QuestConfig : ScriptableObject
{
    [SerializeField] internal string _key;
    [SerializeField] internal string _name;
    [SerializeField] internal string _description;
    [SerializeField] internal ItemConfig _item;
    [SerializeField] internal int _count;
    [SerializeField] internal QuestConfig _nextQuest;

    internal bool IsDone(EntityMonobeh entityMonobeh)
    {
        var inventory = entityMonobeh.EntityInProcess.EntityData.GetComponent<ComponentInventory>();
        var currentCount = inventory.GetCountOfItem(_item.Key);
        return currentCount >= _count;
    }

    internal string GetState(EntityMonobeh entityMonobeh)
    {
        var inventory = entityMonobeh.EntityInProcess.EntityData.GetComponent<ComponentInventory>();
        var currentCount = inventory.GetCountOfItem(_item.Key);
        return $"Have {_item.Key}:{currentCount}/{_count}";
    }
}