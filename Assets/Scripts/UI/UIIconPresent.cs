using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.EventSystems;

public class UIIconPresent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDragHandler, IDropHandler, IPointerClickHandler, IBeginDragHandler
{
    [SerializeField] private AspectRatioFitter _aspectRatioFitter;
    [SerializeField] private Button _button;
    [SerializeField] private Image _colorBorder;
    [SerializeField] private Image _colorBackground;
    [SerializeField] private Image _iconItem;
    [SerializeField] private Image _iconTypeItem;
    [SerializeField] private TextMeshProUGUI _textBottom;
    [SerializeField] private RectTransform rectTransform;

    private int _indexIcon;
    public Action<int, long> OnClickIconByEntity;
    public Action<int> OnClickIcon;
    public Action<int> OnClickMBM;
    public Action<int> OnPointerEnter;
    public Action<int> OnPointerExit;
    /// <summary>
    /// Схватить
    /// </summary>
    public Action<int> OnDragHandler;
    /// <summary>
    /// Отпустить
    /// </summary>
    public Action<int> OnDropHandler;
    public Action<int, long> OnDragHandlerByEntity;
    public Action<int, long> OnDropHandlerByEntity;

    private void Awake()
    {
        _button.onClick.AddListener(OnClick);
    }

    public void InitIcon(UIIconModel iconModel)
    {
        _indexIcon = iconModel.Index;
        _colorBackground.color = iconModel.ColorBackGround;
        _iconItem.sprite = iconModel.Icon;
        _iconTypeItem.sprite = iconModel.IconType;
        _iconTypeItem.enabled = iconModel.Icon == null;
        _textBottom.text = iconModel.BottomText;
        _aspectRatioFitter.aspectMode = iconModel.AspectMode;
        _colorBorder.color = iconModel.ClickableIcon ? Color.white : Color.gray;
//        rectTransform.sizeDelta = new Vector2(50f, 50f);
    }

    private void OnClick()
    {
        OnClickIcon?.Invoke(_indexIcon);
        OnClickIconByEntity?.Invoke(_indexIcon, UIPlayerManager.Instance.EntityMonobeh.Id);
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        UIPlayerManager.ISCURSORUNDERUI = true;
           OnPointerEnter?.Invoke(_indexIcon);
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        UIPlayerManager.ISCURSORUNDERUI = false;
        OnPointerExit?.Invoke(_indexIcon);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        OnDragHandler?.Invoke(_indexIcon);
        OnDragHandlerByEntity?.Invoke(_indexIcon, UIPlayerManager.Instance.EntityMonobeh.Id);
    }

    public void OnDrag(PointerEventData eventData)
    {
//        OnDragHandler?.Invoke(_indexIcon);
//        OnDragHandlerByEntity?.Invoke(_indexIcon, UIPlayerManager.Instance.EntityMonobeh.Id);
    }

    public void OnDrop(PointerEventData eventData)
    {
        OnDropHandler?.Invoke(_indexIcon);
        OnDropHandlerByEntity?.Invoke(_indexIcon, UIPlayerManager.Instance.EntityMonobeh.Id);
    }

    private void OnDestroy()
    {
        _button.onClick.RemoveAllListeners();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Middle)
        {
            OnClickMBM?.Invoke(_indexIcon);
        }
    }
}

public class UIIconModel
{
    public int Index;
    public Sprite Icon;
    public Sprite IconType;
    public Color ColorBackGround;
    public string BottomText;
    public AspectRatioFitter.AspectMode AspectMode;
    public bool ClickableIcon = false;

    /// <summary>
    /// for build
    /// </summary>
    /// <param name="icon"></param>
    public UIIconModel(Sprite icon)
    {
        Icon = icon;
        ColorBackGround = Color.white;
        BottomText = string.Empty;
        AspectMode = AspectRatioFitter.AspectMode.WidthControlsHeight;
    }

    public UIIconModel(ItemData item)
    {
        var conf = item.ItemConfig;

        Icon = conf.GetSprite(0);
        IconType = ItemTypeIconLibrary.Instance.GetIcon(item.CATEGORYOFSLOT);
        ColorBackGround = conf.ColorBackGround;
        BottomText = item.Count > 0 ? $"{item.Count}" : string.Empty;
        AspectMode = AspectRatioFitter.AspectMode.HeightControlsWidth;
    }

    public UIIconModel(int index, ItemData item)
    {
        var conf = item.ItemConfig;

        Index = index;
        Icon = conf.GetSprite(0);
        IconType = ItemTypeIconLibrary.Instance.GetIcon(item.CATEGORYOFSLOT);
        ColorBackGround = conf.ColorBackGround;
        BottomText = item.Count > 0 ? $"{item.Count}" : string.Empty;
        AspectMode = AspectRatioFitter.AspectMode.HeightControlsWidth;
        ClickableIcon = conf.IsUseLess;
    }

    public UIIconModel(ElementRecipe recipe)
    {
        Icon = recipe.ItemConfig.GetSprite(0);
        ColorBackGround = Color.white;
        BottomText = recipe.Count > 1 ? $"{recipe.Count}" : string.Empty;
        AspectMode = AspectRatioFitter.AspectMode.WidthControlsHeight;
    }

    public UIIconModel(ElementRecipe recipe, bool isHaveResource, int index)
    {
        Index = index;
        Icon = recipe.ItemConfig.GetSprite(0);
        ColorBackGround = isHaveResource ? Color.white : Color.gray;
        BottomText = string.Empty;
        AspectMode = AspectRatioFitter.AspectMode.WidthControlsHeight;
    }

    public UIIconModel(ElementRecipe recipe, int currentCount, AspectRatioFitter.AspectMode aspectMode = AspectRatioFitter.AspectMode.WidthControlsHeight)
    {
        Index = 0;
        Icon = recipe.ItemConfig.GetSprite(0);
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