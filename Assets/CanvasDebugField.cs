using TMPro;
using UnityEngine;

public class CanvasDebugField : PrefabByComponentData
{
    [SerializeField] private TextMeshProUGUI _testText;
    [SerializeField] private PrefabByComponentData _spectatorComponent;

    private void Update()
    {
        _testText.text = _spectatorComponent.GetDebugText;
    }
}