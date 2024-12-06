using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Linq;

public class UIPanelCraft : MonoBehaviour
{
    [SerializeField] private UIIconPresent _prefabResources;

    [SerializeField] private TextMeshProUGUI _titleRecipe;
    [SerializeField] private UIIconPresent _iconResult;
    [SerializeField] private Transform _parentResources;
    [SerializeField] private Button _buttonCraft;

    private RecipeSO _recipe;
//    private ComponentInventory _componentInventory;

    public Action OnApplyRecipe;

    private void Awake()
    {
        _buttonCraft.onClick.AddListener(OnClick);
    }

    public void Init(RecipeSO recipe)//, ComponentInventory componentInventory)
    {
//        _componentInventory = componentInventory;

        _recipe = recipe;

        _titleRecipe.text = recipe.TitleRecipe;

        UIIconModel iconModel = recipe.IconModelResult;
        _iconResult.InitIcon(iconModel);

        UpdatePanel();
    }

    private void UpdatePanel()
    {
        var entDat = UIPlayerManager.Instance.EntityMonobeh.EntityInProcess.EntityData;
        _buttonCraft.interactable = _recipe.AvailableRecipe(entDat);
//            _componentInventory.AvailableRecipe(_recipe);

        _parentResources.DestroyChildrens();
        foreach (var r in _recipe.Resources)
        {
            var sum = entDat.Components.Where(c => c is ComponentInventory).Sum(i => ((ComponentInventory)i).GetCountOfItem(r.ItemConfig.Key));

            var uicp = Instantiate(_prefabResources, _parentResources);
            uicp.InitIcon(new UIIconModel(r, sum, aspectMode: AspectRatioFitter.AspectMode.HeightControlsWidth));
        }
    }

    private void OnClick()
    {
        if (!_recipe.AvailableRecipe(UIPlayerManager.Instance.EntityMonobeh.EntityInProcess.EntityData))
        {
            return;
        }

        if (_recipe is RecipeEntitySpawn res)
        {
            UIPlayerManager.Instance.RunPlanBuild(res);
        }
        else
        {
            var compInv = UIPlayerManager.Instance.EntityMonobeh.PrefabsByComponents.GetComponent<BaseInventoryAdapter>();
            var cmdSetter = compInv.GetCommandUseRecipe(UIPlayerManager.Instance.EntityMonobeh.EntityInProcess.EntityData, _recipe, Vector3.one);
            UIPlayerManager.Instance.EntityMonobeh.EntityInProcess.SendCommand(cmdSetter);
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