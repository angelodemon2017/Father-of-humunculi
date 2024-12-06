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

    private List<GroupSO> _tempGroups = new();

    private VisorComponent _visorComponent;

    private List<UIIconPresent> _tempIcons = new();

    public Action OnApplyCraft;

    private void Awake()
    {
        _panelCraftItems.OnApplyCraft += UpdateMarks;
    }

    public void Init(EntityMonobeh EM)
    {
        _visorComponent = EM.GetMyComponent<VisorComponent>();
        _visorComponent.OnChangedEntities += UpdateAvailableEnts;

        _parentButtons.DestroyChildrens();
        for (int g = 0; g < _groupPlayer.Count; g++)
        {
            var uicp = Instantiate(_prefabRecipeIcon, _parentButtons);

            uicp.InitIcon(new UIIconModel(_groupPlayer[g], g));
            uicp.OnPointerEnter += SelectGroup;
        }
    }

    public void UpdateMarks()
    {
        OnApplyCraft?.Invoke();
    }

    private void SelectGroup(int i)
    {
        _panelCraftItems.gameObject.SetActive(true);
        _panelCraftItems.Init(i < _groupPlayer.Count ? _groupPlayer[i].GroupName : _tempGroups[i - _groupPlayer.Count].GroupName);
        _panelHider.gameObject.SetActive(true);
        _panelHider.AddGO(_panelCraftItems.gameObject);
    }

    private void UpdateAvailableEnts()
    {
        _tempGroups.Clear();
        foreach (var ve in _visorComponent.VEs)
        {
            var cmp = ve.Root.GetMyComponent<AvailablerGroupRecipe>();
            if (cmp != null)
            {
                _tempGroups.Add(cmp.GetAvailableGroup);
            }
        }

        if (_tempIcons.Count == _tempGroups.Count)
            return;

        foreach (var ti in _tempIcons)
        {
            Destroy(ti.gameObject);
        }
        _tempIcons.Clear();

        for (int g = 0; g < _tempGroups.Count; g++)
        {
            var uicp = Instantiate(_prefabRecipeIcon, _parentButtons);

            uicp.InitIcon(new UIIconModel(_tempGroups[g], g + _groupPlayer.Count));
            uicp.OnPointerEnter += SelectGroup;

            _tempIcons.Add(uicp);
        }
    }

    private void OnDestroy()
    {
        _panelCraftItems.OnApplyCraft -= UpdateMarks;
    }
}