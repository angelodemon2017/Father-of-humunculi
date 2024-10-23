using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPanelCraftItems : MonoBehaviour
{
    [SerializeField] private PanelHider _panelHider;
    [SerializeField] private UIIconPresent _prefabRecipeIcon;
    [SerializeField] private Transform _parentIconRecipes;
    [SerializeField] private UIPanelCraft _uIPanelCraft;
    [SerializeField] private VerticalLayoutGroup _verticalLayoutGroup;

    private List<RecipeSO> recipes = new();

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public void Init(string groupName)
    {
        _parentIconRecipes.DestroyChildrens();
        var tempRecipes = RecipesController.GetRecipes(groupName);
        foreach (var r in tempRecipes)
        {
            var uicp = Instantiate(_prefabRecipeIcon, _parentIconRecipes);

            uicp.InitIcon(new UIIconModel(r.Result));
//            uicp.OnClickIcon += ClickRecipe;
//            uicp.OnPointerEnter += ClickRecipe;
            uicp.SetPointerAction(SelectRecipe);

            recipes.Add(r);
        }

        _verticalLayoutGroup.enabled = false;
        _verticalLayoutGroup.enabled = true;
    }

    private void SelectRecipe(int index)
    {
        Debug.Log($"ClickRecipe {index}");

        _uIPanelCraft.gameObject.SetActive(true);
        _uIPanelCraft.Init(recipes[index]);
        _panelHider.AddGO(_uIPanelCraft.gameObject);
    }
}