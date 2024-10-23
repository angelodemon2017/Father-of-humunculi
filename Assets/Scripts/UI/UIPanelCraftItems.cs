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

    private List<RecipeSO> _recipes = new();
    private ComponentInventory _componentInventory;
    private UIPanelCraft _tempPanelCraft;
    private string _selectGroupRecipe;

    public Action OnApplyCraft;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public void Init(string groupName, ComponentInventory componentInventory)
    {
        _selectGroupRecipe = groupName;
        _componentInventory = componentInventory;

        _parentCraftPanel.DestroyChildrens();

        UpdateUI();

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
        _parentCraftPanel.DestroyChildrens();

        if (_tempPanelCraft != null)
        {
            _tempPanelCraft.OnApplyRecipe -= WasClickCraft;
        }

        _tempPanelCraft = Instantiate(_uIPanelCraftPrefab, _parentCraftPanel);

        _tempPanelCraft.Init(_recipes[index], _componentInventory);
        _tempPanelCraft.OnApplyRecipe += WasClickCraft;
    }

    private void UpdateUI()
    {
        _recipes.Clear();
        _parentIconRecipes.DestroyChildrens();
        var tempRecipes = RecipesController.GetRecipes(_selectGroupRecipe);
        for (int r = 0; r < tempRecipes.Count; r++)
        {
            var uicp = Instantiate(_prefabRecipeIcon, _parentIconRecipes);

            uicp.InitIcon(new UIIconModel(tempRecipes[r].Result, _componentInventory.AvailableRecipe(tempRecipes[r]), r));
            uicp.OnPointerEnter += SelectRecipe;

            _recipes.Add(tempRecipes[r]);
        }

    }

    private void WasClickCraft()
    {
        UpdateUI();
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