using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPanelCraftItems : MonoBehaviour
{
    [SerializeField] private UIPanelCraft _uIPanelCraftPrefab;
    [SerializeField] private PanelHider _panelHider;
    [SerializeField] private UIIconPresent _prefabRecipeIcon;
    [SerializeField] private Transform _parentIconRecipes;
    [SerializeField] private Transform _parentCraftPanel;
    [SerializeField] private VerticalLayoutGroup _verticalLayoutGroup;

    private List<UIIconPresent> _tempIcons = new();
    private List<RecipeSO> _recipes = new();
//    private ComponentInventory _componentInventory;
    private UIPanelCraft _tempPanelCraft;
    private string _selectGroupRecipe;

    private int _tempFocusIndex;

    public Action OnApplyCraft;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public void Init(string groupName)
    {
        _selectGroupRecipe = groupName;

        _parentCraftPanel.DestroyChildrens();

        InitIcons();

        StartCoroutine(Crunch());
    }

    private IEnumerator Crunch()
    {
        yield return new WaitForSeconds(0.1f);
        _parentIconRecipes.gameObject.SetActive(false);
        _parentIconRecipes.gameObject.SetActive(true);
    }

    private void SelectRecipe(int index)
    {
        _tempFocusIndex = index;
        _parentCraftPanel.DestroyChildrens();

        if (_tempPanelCraft != null)
        {
            _tempPanelCraft.OnApplyRecipe -= WasClickCraft;
        }

        if (_recipes[index] is RecipeResearch rr)
        {
            if (ResearchLibrary.Instance.IsResearchComplete(rr.research.Name))
            {
                return;
            }
        }

        _tempPanelCraft = Instantiate(_uIPanelCraftPrefab, _parentCraftPanel);

        _tempPanelCraft.Init(_recipes[index]);
        _tempPanelCraft.OnApplyRecipe += WasClickCraft;
    }

    private void CraftRecipe(int index)
    {
        if (!_recipes[index].PlayerUseRecipe())
        {
            return;
        }

        UpdateIcons();
    }

    private void InitIcons()
    {
        _recipes.Clear();
        _parentIconRecipes.DestroyChildrens();
        _tempIcons.Clear();
        var tempRecipes = RecipesController.GetRecipes(_selectGroupRecipe);
        for (int r = 0; r < tempRecipes.Count; r++)
        {
            var uicp = Instantiate(_prefabRecipeIcon, _parentIconRecipes);

            UIIconModel iconModel = tempRecipes[r].IconModelResult;
            iconModel.Index = r;
            uicp.InitIcon(iconModel);
            uicp.OnPointerEnter += SelectRecipe;
            uicp.OnClickIcon += CraftRecipe;

            _tempIcons.Add(uicp);
            _recipes.Add(tempRecipes[r]);
        }
    }

    private void UpdateIcons()
    {
        var tempRecipes = RecipesController.GetRecipes(_selectGroupRecipe);
        for (int r = 0; r < tempRecipes.Count; r++)
        {
            UIIconModel iconModel = tempRecipes[r].IconModelResult;
            iconModel.Index = r;
            _tempIcons[r].InitIcon(iconModel);
        }
        _tempPanelCraft.Init(_recipes[_tempFocusIndex]);
    }

    private void WasClickCraft()
    {
        UpdateIcons();
        OnApplyCraft?.Invoke();
    }

    private void OnDisable()
    {
        if (_tempPanelCraft != null)
        {
            _tempPanelCraft.OnApplyRecipe -= WasClickCraft;
        }
        _parentCraftPanel.DestroyChildrens();
    }
}