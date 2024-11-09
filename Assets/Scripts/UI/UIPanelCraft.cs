using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UIPanelCraft : MonoBehaviour
{
    [SerializeField] private UIIconPresent _prefabResources;

    [SerializeField] private TextMeshProUGUI _titleRecipe;
    [SerializeField] private UIIconPresent _iconResult;
    [SerializeField] private Transform _parentResources;
    [SerializeField] private Button _buttonCraft;

    private RecipeSO _recipe;
    private ComponentInventory _componentInventory;

    public Action OnApplyRecipe;

    private void Awake()
    {
        _buttonCraft.onClick.AddListener(OnClick);
    }

    public void Init(RecipeSO recipe, ComponentInventory componentInventory)
    {
        _componentInventory = componentInventory;

        _recipe = recipe;

        _titleRecipe.text = recipe.TitleRecipe;

        UIIconModel iconModel = recipe.IconModelResult;
        _iconResult.InitIcon(iconModel);

        UpdatePanel();
    }

    private void UpdatePanel()
    {
        _buttonCraft.interactable = _componentInventory.AvailableRecipe(_recipe);

        _parentResources.DestroyChildrens();
        foreach (var r in _recipe.Resources)
        {
            var uicp = Instantiate(_prefabResources, _parentResources);
            uicp.InitIcon(new UIIconModel(r, _componentInventory.GetCountOfItem(r.ItemConfig.Key), aspectMode: AspectRatioFitter.AspectMode.HeightControlsWidth));
        }
    }

    private void OnClick()
    {
        if (_recipe.IsBuild)
        {
            UIPlayerManager.Instance.RunPlanBuild(_recipe);
        }
        else
        {
            _componentInventory.TryApplyRecipe(_recipe);
            //TODO MEGA CRUNCH, need use send command to entity
            UIPlayerManager.Instance.EntityMonobeh.EntityInProcess.EntityData.UpdateEntity();
        }

        UpdatePanel();
        OnApplyRecipe?.Invoke();
    }

    private void OnDisable()
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        _buttonCraft.onClick.RemoveAllListeners();
    }
}