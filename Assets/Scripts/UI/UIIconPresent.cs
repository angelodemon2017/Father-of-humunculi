using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.EventSystems;

public class UIIconPresent : MonoBehaviour, IPointerEnterHandler, IDragHandler, IDropHandler
{
    [SerializeField] private AspectRatioFitter _aspectRatioFitter;
    [SerializeField] private Button _button;
    [SerializeField] private Image _colorBackground;
    [SerializeField] private Image _iconItem;
    [SerializeField] private TextMeshProUGUI _textBottom;
    [SerializeField] private RectTransform rectTransform;

    private int _indexIcon;
    public Action<int> OnClickIcon;
    public Action<int> OnPointerEnter;
    public Action<int> OnDragHandler;
    public Action<int> OnDropHandler;

    public void SetPointerAction(Action<int> action)
    {
        OnPointerEnter = action;
    }

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
        _aspectRatioFitter.aspectMode = iconModel.AspectMode;
//        rectTransform.sizeDelta = new Vector2(50f, 50f);
    }

    private void OnClick()
    {
        OnClickIcon?.Invoke(_indexIcon);
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        OnPointerEnter?.Invoke(_indexIcon);
    }

    public void OnDrag(PointerEventData eventData)
    {
        OnDragHandler?.Invoke(_indexIcon);
    }

    public void OnDrop(PointerEventData eventData)
    {
        OnDropHandler?.Invoke(_indexIcon);
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
    public AspectRatioFitter.AspectMode AspectMode;

    public UIIconModel(int index, ItemData item)
    {
        var conf = ItemsController.GetItem(item.EnumId);

        Index = index;
        Icon = conf.IconItem;
        ColorBackGround = conf.ColorBackGround;
        BottomText = item.Count > 0 ? $"{item.Count}" : string.Empty;
        AspectMode = AspectRatioFitter.AspectMode.HeightControlsWidth;
    }

    public UIIconModel(ElementRecipe recipe)
    {
        Icon = recipe.ItemConfig.IconItem;
        ColorBackGround = Color.white;
        BottomText = recipe.Count > 1 ? $"{recipe.Count}" : string.Empty;
        AspectMode = AspectRatioFitter.AspectMode.WidthControlsHeight;
    }

    public UIIconModel(ElementRecipe recipe, bool isHaveResource, int index)
    {
        Index = index;
        Icon = recipe.ItemConfig.IconItem;
        ColorBackGround = isHaveResource ? Color.white : Color.gray;
        BottomText = string.Empty;
        AspectMode = AspectRatioFitter.AspectMode.WidthControlsHeight;
    }

    public UIIconModel(ElementRecipe recipe, int currentCount, AspectRatioFitter.AspectMode aspectMode = AspectRatioFitter.AspectMode.WidthControlsHeight)
    {
        Index = 0;
        Icon = recipe.ItemConfig.IconItem;
        ColorBackGround = currentCount >= recipe.Count ? Color.white : Color.gray;
        BottomText = $"{currentCount}/{recipe.Count}";
        AspectMode = aspectMode;
    }

    public UIIconModel(GroupSO group, int index)
    {
        Index = index;
        Icon = group.IconGroup;
        ColorBackGround = Color.white;
        BottomText = string.Empty;
        AspectMode = AspectRatioFitter.AspectMode.WidthControlsHeight;
    }
}