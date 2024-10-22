using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelHider : MonoBehaviour
{
    [SerializeField] private Button ButtonPanel;
    [SerializeField] private List<GameObject> Panels = new();

    private void Awake()
    {
        ButtonPanel.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        Panels.ForEach(p => p.SetActive(false));
    }

    private void OnDestroy()
    {
        ButtonPanel.onClick.RemoveAllListeners();
    }
}