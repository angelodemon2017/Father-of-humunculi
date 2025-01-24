using System;
using System.Collections.Generic;

internal class QuestDataController
{
    public static Action updateQuests;

    internal List<QuestData> ActivQuests = new List<QuestData>();

    public void AddQuest(QuestConfig questConfig)
    {
        ActivQuests.Add(new QuestData() 
        {
            Key = questConfig._key,
        });
        updateQuests?.Invoke();
    }

    
}