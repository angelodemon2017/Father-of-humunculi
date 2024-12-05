using System;
using System.Collections.Generic;
using UnityEngine;

public class UIPanelCraftGroups : MonoBehaviour
{
    [SerializeField] private PanelHider _panelHider;
    [SerializeField] private UIIconPresent _prefabRecipeIcon;
    [SerializeField] private Transform _parentButtons;
    [SerializeField] private UIPanelCraftItems _panelCraftItems;
    [SerializeField] private List<GroupSO> _groupPlayer;

//    private ComponentInventory _componentInventory;
    private List<UIIconPresent> _tempIcons = new();//??
    private string _focusEntity = string.Empty;

    public Action OnApplyCraft;

    private void Awake()
    {
        _panelCraftItems.OnApplyCraft += UpdateMarks;
    }

    public void Init()//ComponentInventory componentInventory)
    {
//        _componentInventory = componentInventory;
        _parentButtons.DestroyChildrens();
        for (int g = 0; g < _groupPlayer.Count; g++)
        {
            var uicp = Instantiate(_prefabRecipeIcon, _parentButtons);

            uicp.InitIcon(new UIIconModel(_groupPlayer[g], g));
            uicp.OnPointerEnter += SelectGroup;

            _tempIcons.Add(uicp);
        }
    }

    public void UpdateMarks()
    {
        OnApplyCraft?.Invoke();
    }

    private void SelectGroup(int i)
    {
        _panelCraftItems.gameObject.SetActive(true);
        _panelCraftItems.Init(_groupPlayer[i].GroupName);//, _componentInventory);
        _panelHider.gameObject.SetActive(true);
        _panelHider.AddGO(_panelCraftItems.gameObject);
    }

    public void AddTempGroup(GroupSO tempGroup)
    {

    }

    private void OnDestroy()
    {
        _panelCraftItems.OnApplyCraft -= UpdateMarks;
    }
}