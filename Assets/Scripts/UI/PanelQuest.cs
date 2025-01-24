using UnityEngine;
using UnityEngine.UI;

public class PanelQuest : MonoBehaviour
{
    [SerializeField] private QuestMiniPanel _prefabQuestMiniPanel;
    [SerializeField] private GameObject _panelShow;
    [SerializeField] private Transform _parentQuests;

    [SerializeField] private Button _buttonForHide;
    [SerializeField] private Button _buttonForShow;

    private bool _showTasks;
    private QuestDataController _questDataController;

    private void Awake()
    {
        _buttonForHide.onClick.AddListener(UpdateShowing);
        _buttonForShow.onClick.AddListener(UpdateShowing);
        UpdateShowing();
    }

    internal void Init()
    {
        _questDataController = GameProcess.Instance.GameWorld._questDataController;
        QuestDataController.UpdateQuests += UpdateQuestList;
    }

    private void UpdateShowing()
    {
        _showTasks = !_showTasks;

        _buttonForHide.gameObject.SetActive(_showTasks);
        _buttonForShow.gameObject.SetActive(!_showTasks);

        _panelShow.SetActive(_showTasks);
    }

    internal void UpdateQuestList()
    {
        _parentQuests.DestroyChildrens();
        foreach (var q in _questDataController.ActivQuests)
        {
            var tempMiniPanel = Instantiate(_prefabQuestMiniPanel, _parentQuests);
            tempMiniPanel.Init(q);
        }
    }

    private void OnDestroy()
    {
        QuestDataController.UpdateQuests -= UpdateQuestList;
    }
}