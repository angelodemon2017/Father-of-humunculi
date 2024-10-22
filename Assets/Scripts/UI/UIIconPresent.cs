using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UIIconPresent : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private Image _colorBackground;
    [SerializeField] private Image _iconItem;
    [SerializeField] private TextMeshProUGUI _textBottom;

    private int _indexIcon;
    public Action<int> OnClickIcon;

    private void Awake()
    {
        _button.onClick.AddListener(OnClick);
    }

    public void InitIcon(UIIconModel iconModel)
    {
        _indexIcon = iconModel.Index;
        _colorBackground.color = iconModel.ColorBackGround;
        _iconItem.sprite = iconModel.Icon;
        _textBottom.text = iconModel.BottomText;
    }

    private void OnClick()
    {
        OnClickIcon?.Invoke(_indexIcon);
    }

    private void OnDestroy()
    {
        _button.onClick.RemoveAllListeners();
    }
}

public class UIIconModel
{
    public int Index;
    public Sprite Icon;
    public Color ColorBackGround;
    public string BottomText;

    public UIIconModel(int index, ItemData item)
    {
        var conf = ItemsController.GetItem(item.EnumId);

        Index = index;
        Icon = conf.IconItem;
        ColorBackGround = conf.ColorBackGround;
        BottomText = item.Count > 0 ? $"{item.Count}" : string.Empty;
    }

    public UIIconModel(ElementRecipe recipe)
    {
        Icon = recipe.ItemConfig.IconItem;
        ColorBackGround = Color.grey;//TODO change color if had resources
        BottomText = recipe.Count > 1 ? $"{recipe.Count}" : string.Empty;
    }
}