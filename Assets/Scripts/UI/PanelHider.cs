using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PanelHider : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] private Button ButtonPanel;
    [SerializeField] private List<GameObject> Panels = new();

    private void Awake()
    {
        ButtonPanel.onClick.AddListener(OnClick);
    }

    public void AddGO(GameObject gameObject)
    {
        Panels.Add(gameObject);
    }

    private void OnClick()
    {
        Panels.ForEach(p => p.SetActive(false));
        Panels.Clear();
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        ButtonPanel.onClick.RemoveAllListeners();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnClick();
    }
}