using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Research Library", order = 1)]
public class ResearchLibrary : ScriptableObject
{
    private static ResearchLibrary _instance;
    public static ResearchLibrary Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Resources.LoadAll<ResearchLibrary>(string.Empty)[0];
            }

            return _instance;
        }
    }

    [SerializeField] private List<ResearchSO> _researches;
    private Dictionary<string, ResearchSO> _cashResearches = new();

    public void Upgrade(RecipeResearch recipeResearch)
    {
        GameProcess.Instance.GameWorld.UpgradeResearch(recipeResearch.research.Name, recipeResearch.CountUpgrade);
    }

    public bool IsResearchComplete(string name)
    {
        Init();

        var currentStatus = GameProcess.Instance.GameWorld.GetStatusResearch(name);
        var needStatus = _cashResearches[name].Need;

        return currentStatus >= needStatus;
    }

    private void Init()
    {
        if (_cashResearches.Count == 0)
        {
            foreach (var ent in _researches)
            {
                _cashResearches.Add(ent.Name, ent);
            }
        }
    }
}