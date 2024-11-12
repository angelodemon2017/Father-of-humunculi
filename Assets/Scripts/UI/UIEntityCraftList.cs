using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIEntityCraftList : PrefabByComponentData
{
    [SerializeField] private GroupSO _recipeGroup;

    [SerializeField] private UIIconPresent _uiIconPresentPrefab;
    [SerializeField] private UIPanelCraft _uiPanelCraftPrefab;

    [SerializeField] private GameObject _root;
    [SerializeField] private TextMeshProUGUI _textTitle;
    [SerializeField] private Button _buttonClose;
    [SerializeField] private Transform _parentButtons;
    [SerializeField] private Transform _parentCraft;

    private UIPanelCraft _tempPanelCraft;
    private List<RecipeSO> _recipes = new();
    private EntityInProcess _entityInProcess;
    private ComponentUICraftGroup _componentUICraftGroup;
    private Transform _whoOpened = null;

    public override string KeyComponent => typeof(UIEntityCraftList).Name;
    public override string KeyComponentData => typeof(ComponentUICraftGroup).Name;

    internal override ComponentData GetComponentData => new ComponentUICraftGroup(_recipeGroup.GroupName);

    private void Awake()
    {
        _root.SetActive(false);
//        _uiPanelCraft.gameObject.SetActive(false);
        _buttonClose.onClick.AddListener(Close);
    }

    public override void Init(ComponentData componentData, EntityInProcess entityInProcess = null)
    {
        _componentUICraftGroup = (ComponentUICraftGroup)componentData;
        _entityInProcess = entityInProcess;

        _entityInProcess.UpdateEIP += UpdateEntity;
        InitUI();
    }

    public override void ExecuteCommand(EntityData entity, string command, string message, WorldData worldData)
    {
        switch (command)
        {
            case Dict.Commands.CloseUI:
                CloseUIData(entity);
                break;
            default:
                break;
        }
    }

    private void CloseUIData(EntityData entity)
    {
        var compPlayer = entity.Components.GetComponent<ComponentUICraftGroup>();
        compPlayer.SetEntityOpener(-1);
        entity.UpdateEntity();
    }

    public void PickEntity(EntityData entity, string command, string message, WorldData worldData)
    {
        var compPlayer = entity.Components.GetComponent<ComponentUICraftGroup>();

        if (compPlayer != null)
        {
            compPlayer.SetEntityOpener(long.Parse(message));
            entity.UpdateEntity();
        }
    }

    private void InitUI()
    {
        _recipes.Clear();
        _textTitle.text = _componentUICraftGroup.RecipeGroup;
        _parentButtons.DestroyChildrens();
        var tempRecipes = RecipesController.GetRecipes(_componentUICraftGroup.RecipeGroup);
        for (int r = 0; r < tempRecipes.Count; r++)
        {
            var uicp = Instantiate(_uiIconPresentPrefab, _parentButtons);

            UIIconModel iconModel = tempRecipes[r].IconModelResult;
            iconModel.Index = r;
            uicp.InitIcon(iconModel);
            uicp.OnClickIcon += SelectRecipe;

            _recipes.Add(tempRecipes[r]);
        }
    }

    private void SelectRecipe(int index)
    {
        var entPlayer = UIPlayerManager.Instance.EntityMonobeh.EntityInProcess.EntityData;
        var tempComponentInventory = entPlayer.Components.GetComponent<ComponentInventory>();

        _tempPanelCraft = Instantiate(_uiPanelCraftPrefab, _parentCraft);

        _tempPanelCraft.gameObject.SetActive(true);
        _tempPanelCraft.Init(_recipes[index], tempComponentInventory);

        _tempPanelCraft.OnApplyRecipe += WasClickCraft;
    }

    private void WasClickCraft()
    {

    }

    private void UpdateEntity()
    {
        var isOpen = _componentUICraftGroup.WhoOpened == UIPlayerManager.Instance.EntityMonobeh.Id;
        if (!isOpen && _tempPanelCraft != null)
        {
            _tempPanelCraft.OnApplyRecipe -= WasClickCraft;
        }
        _root.SetActive(isOpen);
        if (_root.activeSelf)
        {
            _whoOpened = UIPlayerManager.Instance.EntityMonobeh.transform;
        }
    }

    private void Close()
    {
        _root.SetActive(false);
        _entityInProcess.SendCommand(GetCommandCloseUI());
    }

    private CommandData GetCommandCloseUI()
    {
        return new CommandData()
        {
            KeyCommand = Dict.Commands.CloseUI,
            KeyComponent = KeyComponent,
        };
    }

    private void FixedUpdate()
    {
        if (_root.activeSelf)
        {
            if (Vector3.Distance(transform.position, _whoOpened.position) > Config.CloseUIDistance)
            {
                Close();
            }
        }
    }

    private void OnDestroy()
    {
        if (_entityInProcess != null)
        {
            _entityInProcess.UpdateEIP -= UpdateEntity;
        }
        _buttonClose.onClick.RemoveAllListeners();
        if (!_root.activeSelf && _tempPanelCraft != null)
        {
            _tempPanelCraft.OnApplyRecipe -= WasClickCraft;
        }
    }
}