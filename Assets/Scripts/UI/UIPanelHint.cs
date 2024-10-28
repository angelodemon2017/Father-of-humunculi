using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class UIPanelHint : MonoBehaviour
{
    [SerializeField] private GameObject _iconRoot;
    [SerializeField] private Image _iconHint;
    [SerializeField] private TextMeshProUGUI _textTitle;
    [SerializeField] private TextMeshProUGUI _textMainDiscription;
    [SerializeField] private TextMeshProUGUI _textUseHints;
    [SerializeField] private VerticalLayoutGroup _verticalLayoutGroup;

    private HintModel _hintModel;

    public void Init(HintModel hintModel)
    {
        _hintModel = hintModel;
        gameObject.SetActive(true);
        UpdateUI();
    }

    private void UpdateUI()
    {
        _iconHint.sprite = _hintModel.Icon;
        _iconRoot.SetActive(_hintModel.Icon != null);
        _textTitle.text = _hintModel.Title;

        _textMainDiscription.text = _hintModel.Description;
        _textMainDiscription.transform.parent.gameObject.SetActive(!string.IsNullOrWhiteSpace(_hintModel.Description));

        _textUseHints.text = _hintModel.UseHints.Count > 0 ? string.Join("\r\n", _hintModel.UseHints) : string.Empty;
        _textUseHints.transform.parent.gameObject.SetActive(_hintModel.UseHints.Count > 0);

/*        _verticalLayoutGroup.enabled = false;
        _verticalLayoutGroup.enabled = true;

        gameObject.SetActive(false);
        gameObject.SetActive(true);/**/
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}

public class HintModel
{
    public Sprite Icon;
    public string Title;
    public string Description;
    public List<string> UseHints = new();
}