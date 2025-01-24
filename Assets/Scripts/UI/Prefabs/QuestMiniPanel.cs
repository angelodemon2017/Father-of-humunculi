using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestMiniPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textQuest;
    [SerializeField] private Button _selfButton;

    private QuestData _questData;

    private void Awake()
    {
        _selfButton.onClick.AddListener(OnClick);
    }

    internal void Init(QuestData questData)
    {
        _questData = questData;
        UpdateDesc();
    }

    private void UpdateDesc()
    {
        _textQuest.text = _questData.Description;
    }

    private void OnClick()
    {

    }

    private void OnDestroy()
    {
        _selfButton.onClick.RemoveAllListeners();
    }
}