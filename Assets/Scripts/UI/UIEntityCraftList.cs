using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIEntityCraftList : PrefabByComponentData
{
    [SerializeField] private GroupSO _recipeGroup;

    [SerializeField] private UIIconPresent _uiIconPresentPrefab;
    [SerializeField] private UIPanelCraft _uiPanelCraftPrefab;

    [SerializeField] private TextMeshProUGUI _textTitle;
    [SerializeField] private Transform _parentButtons;
    [SerializeField] private Transform _parentCraft;

    private UIPanelCraft _tempPanelCraft;
    private List<UIIconPresent> _tempIcons = new();
    private List<RecipeSO> _recipes = new();
    private EntityInProcess _entityInProcess;
    private ComponentUICraftGroup _componentUICraftGroup;

    public override string KeyComponentData => typeof(ComponentUICraftGroup).Name;

    internal override ComponentData GetComponentData => new ComponentUICraftGroup(_recipeGroup.GroupName);

    public override void Init(ComponentData componentData, EntityInProcess entityInProcess = null)
    {
        _componentUICraftGroup = (ComponentUICraftGroup)componentData;
        _entityInProcess = entityInProcess;

        _entityInProcess.UpdateEIP += UpdateEntity;
        InitUI();
    }

    private void InitUI()
    {
        _recipes.Clear();
        _textTitle.text = _componentUICraftGroup.RecipeGroup;
        _parentButtons.DestroyChildrens();
        _tempIcons.Clear();
        var tempRecipes = RecipesController.GetRecipes(_componentUICraftGroup.RecipeGroup);
        for (int r = 0; r < tempRecipes.Count; r++)
        {
            var uicp = Instantiate(_uiIconPresentPrefab, _parentButtons);

            UIIconModel iconModel = tempRecipes[r].IconModelResult;
            iconModel.Index = r;
            uicp.InitIcon(iconModel);
            uicp.OnClickIcon += SelectRecipe;

            _tempIcons.Add(uicp);
            _recipes.Add(tempRecipes[r]);
        }
    }

    private void SelectRecipe(int index)
    {
        var entPlayer = UIPlayerManager.Instance.EntityMonobeh.EntityInProcess.EntityData;
        var tempComponentInventory = entPlayer.Components.GetComponent<ComponentInventory>();

        _tempPanelCraft = Instantiate(_uiPanelCraftPrefab, _parentCraft);

        _tempPanelCraft.gameObject.SetActive(true);
        _tempPanelCraft.Init(_recipes[index]);//, tempComponentInventory);

        _tempPanelCraft.OnApplyRecipe += WasClickCraft;
        UpdateIcons();
    }

    private void UpdateIcons()
    {
        var tempRecipes = RecipesController.GetRecipes(_componentUICraftGroup.RecipeGroup);
        for (int r = 0; r < tempRecipes.Count; r++)
        {
            UIIconModel iconModel = tempRecipes[r].IconModelResult;
            iconModel.Index = r;
            _tempIcons[r].InitIcon(iconModel);
        }
    }

    private void WasClickCraft()
    {
        UpdateIcons();
    }

    private void UpdateEntity()
    {
        if (_tempPanelCraft != null)
        {
            _tempPanelCraft.OnApplyRecipe -= WasClickCraft;
        }
    }

    private void OnDisable()
    {
//        _tempPanelCraft.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        _tempIcons.Clear();
        if (_entityInProcess != null)
        {
            _entityInProcess.UpdateEIP -= UpdateEntity;
        }
        if (_tempPanelCraft != null)
        {
            _tempPanelCraft.OnApplyRecipe -= WasClickCraft;
        }
    }
}