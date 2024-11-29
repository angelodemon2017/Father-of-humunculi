using TMPro;
using UnityEngine;

public class CanvasUIlabels : PrefabByComponentData
{
    [SerializeField] private TextMeshProUGUI _testText;
    [SerializeField] private PrefabByComponentData GetText;

    internal override bool _isNeedUpdate => true;
    public override string KeyComponentData => typeof(ComponentUIlabels).Name;
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