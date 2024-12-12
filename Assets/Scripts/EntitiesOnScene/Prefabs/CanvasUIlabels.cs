using TMPro;
using UnityEngine;
using static OptimazeExtensions;

public class CanvasUIlabels : PrefabByComponentData
{
    public override int KeyType => TypeCache<CanvasUIlabels>.IdType;
    [SerializeField] private TextMeshProUGUI _testText;
    [SerializeField] private PrefabByComponentData GetText;

    internal override bool _isNeedUpdate => true;
    public override int KeyComponentData => TypeCache<ComponentUIlabels>.IdType;
    internal override ComponentData GetComponentData => new ComponentUIlabels();

    public override void Init(ComponentData componentData, EntityInProcess entityInProcess = null)
    {

    }

    internal override void UpdateComponent()
    {
        TestCheckText();
    }

    private void TestCheckText()
    {
        _testText.text = GetText != null ? GetText.GetDebugText : string.Empty;
    }
}