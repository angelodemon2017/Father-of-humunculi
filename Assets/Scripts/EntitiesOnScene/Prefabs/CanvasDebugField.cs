using TMPro;
using UnityEngine;
using static OptimazeExtensions;

public class CanvasDebugField : PrefabByComponentData
{
    public override int KeyType => TypeCache<CanvasDebugField>.IdType;
    [SerializeField] private TextMeshProUGUI _testText;
    [SerializeField] private PrefabByComponentData _spectatorComponent;

    private void Update()
    {
        _testText.text = _spectatorComponent.GetDebugText;
    }
}