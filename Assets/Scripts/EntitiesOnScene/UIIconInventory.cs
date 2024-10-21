using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UIIconInventory : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private Image _colorBackground;
    [SerializeField] private Image _iconItem;
    [SerializeField] private TextMeshProUGUI _textCount;

    private ItemData _item;//??

    public Action<ItemData> OnClickItem;//TODO needCommand

    public void Init(ItemData item)
    {
        var iConf = ItemsController.GetItem(item.EnumId);
        _button.onClick.AddListener(OnClick);

        _item = item;

        UpdateIcon();
    }

    private void OnClick()
    {
        if (_item == null || _item.EnumId == EnumItem.None)
        {
            return;
        }

        OnClickItem?.Invoke(_item);
    }

    private void UpdateIcon()
    {
        var iConf = ItemsController.GetItem(_item.EnumId);

        _colorBackground.color = iConf.ColorBackGround;
        _iconItem.sprite = iConf.IconItem;
        _textCount.text = _item.Count > 0 ? $"{ _item.Count}" : string.Empty;
    }

    public void InitEmpty()
    {
        _item = new ItemData(ItemsController.GetItem(EnumItem.None));
        UpdateIcon();
    }

    private void OnDestroy()
    {
        _button.onClick.RemoveAllListeners();
    }
}