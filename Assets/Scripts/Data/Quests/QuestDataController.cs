using System;
using System.Collections.Generic;

internal class QuestDataController
{
    public static Action UpdateQuests;

    internal List<QuestData> ActivQuests = new List<QuestData>();

    public void AddQuest(QuestConfig questConfig)
    {
        ActivQuests.Add(questConfig.GetQuestData);
        UpdateQuests?.Invoke();
    }

    private List<QuestData> _questsForDel = new();
    private List<QuestData> _questsForAdd = new();
    internal void CheckQuests()
    {
        foreach (var q in ActivQuests)
        {
            if (q.IsDone)
            {
                if (q.GetConfig._nextQuest != null)
                {
                    _questsForAdd.Add(q.GetConfig._nextQuest.GetQuestData);
                }
                _questsForDel.Add(q);
            }
        }

        _questsForDel.ForEach(q => ActivQuests.Remove(q));
        _questsForDel.Clear();

        _questsForAdd.ForEach(q => ActivQuests.Add(q));
        _questsForAdd.Clear();

        UpdateQuests?.Invoke();
    }
}