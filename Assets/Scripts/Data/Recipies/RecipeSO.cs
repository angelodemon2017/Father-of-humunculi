using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Recipies/Recipe orig", order = 1)]
public class RecipeSO : ScriptableObject
{
    [HideInInspector]
    public int Index;

//    public ElementRecipe Result;

    public List<ElementRecipe> Resources = new();
    public List<ResearchSO> needResearchs = new();

    public GroupSO GroupRecipeTag;

    public virtual UIIconModel IconModelResult => null;// new UIIconModel();
    public virtual string TitleRecipe => string.Empty;// Result.ItemConfig.Key;

    internal virtual bool AvailableRecipe(EntityData entityData)
    {
        foreach (var nr in needResearchs)
        {
            if (!ResearchLibrary.Instance.IsResearchComplete(nr.Name))
            {
                return false;
            }
        }

        var invs = entityData.Components.GetComponents(typeof(ComponentInventory).Name);
        foreach (var r in Resources)
        {
            if (invs.Sum(i => ((ComponentInventory)i).GetCountOfItem(r.ItemConfig.Key)) >= r.Count)
            {
                continue;
            }
            else
            {
                return false;
            }
        }

        return true;
    }

    internal virtual void UseAndReleaseRecipe(EntityData entityData, string args = "")
    {
        UseResources(entityData);
        ReleaseRecipe(entityData, args);
    }

    internal virtual void ReleaseRecipe(EntityData entityData, string args = "")
    {

    }

    internal virtual void UseResources(EntityData entityData)
    {
        var invs = entityData.Components.GetComponents(typeof(ComponentInventory).Name);
        foreach (var r in Resources)
        {
            var needSubtract = r.Count;
            foreach (ComponentInventory i in invs)
            {
                var availRes = i.GetCountOfItem(r.ItemConfig.Key);
                i.SubtrackItems(r.ItemConfig.Key, availRes >= needSubtract ? needSubtract : availRes);
                needSubtract -= availRes;
                if (needSubtract <= 0)
                {
                    break;
                }
            }
        }
    }
}