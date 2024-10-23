using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIPanelCraftGroups : MonoBehaviour
{
    [SerializeField] private PanelHider _panelHider;
    [SerializeField] private UIIconPresent _prefabRecipeIcon;
    [SerializeField] private Transform _parentButtons;
    [SerializeField] private UIPanelCraftItems _panelCraftItems;
    [SerializeField] private VerticalLayoutGroup _verticalLayoutGroup;

    private List<GroupSO> _groups = new();

    private void Awake()
    {
        _parentButtons.DestroyChildrens();
        var groups = GroupController.GetAllGroups().OrderBy(g => g.Order).ToList();
        for (int g = 0; g < groups.Count; g++)
        {
            var uicp = Instantiate(_prefabRecipeIcon, _parentButtons);

            uicp.InitIcon(new UIIconModel(groups[g], g));
//            uicp.OnClickIcon += Click;
            uicp.OnPointerEnter += Click;

            _groups.Add(groups[g]);
        }
    }

    private void Click(int i)
    {
        _panelCraftItems.gameObject.SetActive(true);
        _panelCraftItems.Init(_groups[i].GroupName);
        _panelHider.gameObject.SetActive(true);
        _panelHider.AddGO(_panelCraftItems.gameObject);
    }
}