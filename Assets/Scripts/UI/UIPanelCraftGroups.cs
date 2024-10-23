using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIPanelCraftGroups : MonoBehaviour
{
    [SerializeField] private PanelHider _panelHider;
    [SerializeField] private UIIconPresent _prefabRecipeIcon;
    [SerializeField] private Transform _parentButtons;
    [SerializeField] private UIPanelCraftItems _panelCraftItems;

    private ComponentInventory _componentInventory;
    private List<GroupSO> _groups = new();

    public Action OnApplyCraft;

    private void Awake()
    {
        _panelCraftItems.OnApplyCraft += UpdateMarks;
    }

    public void Init(ComponentInventory componentInventory)
    {
        _componentInventory = componentInventory;
        _parentButtons.DestroyChildrens();
        var groups = GroupController.GetAllGroups().OrderBy(g => g.Order).ToList();
        for (int g = 0; g < groups.Count; g++)
        {
            var uicp = Instantiate(_prefabRecipeIcon, _parentButtons);

            uicp.InitIcon(new UIIconModel(groups[g], g));
            uicp.OnPointerEnter += SelectGroup;

            _groups.Add(groups[g]);
        }
    }

    public void UpdateMarks()
    {
        OnApplyCraft?.Invoke();
    }

    private void SelectGroup(int i)
    {
        _panelCraftItems.gameObject.SetActive(true);
        _panelCraftItems.Init(_groups[i].GroupName, _componentInventory);
        _panelHider.gameObject.SetActive(true);
        _panelHider.AddGO(_panelCraftItems.gameObject);
    }

    private void OnDestroy()
    {
        _panelCraftItems.OnApplyCraft -= UpdateMarks;
    }
}