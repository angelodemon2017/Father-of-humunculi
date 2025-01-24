using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class QuestLibrary : MonoBehaviour
{
    public static QuestLibrary Instance;

    [SerializeField] private List<QuestConfig> questConfigs;

    private void Awake()
    {
        Instance = this;
    }

    internal QuestConfig GetQuestConfigByKey(string key)
    {
        return questConfigs.FirstOrDefault(x => x._key == key);
    }
}