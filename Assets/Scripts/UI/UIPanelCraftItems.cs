using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPanelCraftItems : MonoBehaviour
{
    [SerializeField] private UIIconPresent _prefabRecipeIcon;
    [SerializeField] private Transform _parentIconRecipes;
    [SerializeField] private UIPanelCraft _uIPanelCraft;

    private List<RecipeSO> recipes = new();

    public void Init(string groupName)
    {
        var tempRecipes = RecipesController.GetRecipes(groupName);
        foreach (var r in tempRecipes)
        {
            var uicp = Instantiate(_prefabRecipeIcon, _parentIconRecipes);

            uicp.InitIcon(new UIIconModel(r.Result));
            uicp.OnClickIcon += ClickRecipe;

            recipes.Add(r);
        }
    }

    private void ClickRecipe(int index)
    {
        _uIPanelCraft.gameObject.SetActive(true);
        _uIPanelCraft.Init(recipes[index]);
    }
}