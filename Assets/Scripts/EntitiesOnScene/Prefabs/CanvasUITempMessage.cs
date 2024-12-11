using TMPro;
using UnityEngine;

public class CanvasUITempMessage : PrefabByComponentData
{
    [SerializeField] private TextMeshProUGUI _testText;
    [SerializeField] private int SecondShows = 1;
    [SerializeField] private string DefaulthMessage;

    private ComponentUITempMessage _component;

    private string GetDefMes => DefaulthMessage;
    internal override bool _isNeedUpdate => true;
    public override string KeyComponentData => typeof(ComponentUITempMessage).Name;
    internal override ComponentData GetComponentData => new ComponentUITempMessage(GetDefMes);

    public override void Init(ComponentData componentData, EntityInProcess entityInProcess = null)
    {
        _component = (ComponentUITempMessage)componentData;
        UpdateText();
    }

    public void ShowMessage(EntityData entity, string command, string message, WorldData worldData)
    {
        var cmp = entity.GetComponent<ComponentUITempMessage>();

        cmp.SecondVision = SecondShows;
        cmp.TextView = message;
        entity.UpdateEntity();
    }

    public void ShowDefMessage(EntityData entity)
    {
        var cmp = entity.GetComponent<ComponentUITempMessage>();
        cmp.SecondVision = SecondShows;
        entity.UpdateEntity();
    }

    internal override void UpdateComponent()
    {
        UpdateText();
    }

    public override void DoSecond(EntityData entity)
    {
        var cmp = entity.GetComponent<ComponentUITempMessage>();

        if (cmp.SecondVision > 0)
        {
            cmp.SecondVision--;
            if (cmp.SecondVision <= 0)
            {
                entity.UpdateEntity();
            }
        }
    }

    private void UpdateText()
    {
        _testText.text = _component.TextView;
        _testText.enabled = _component.IsShowMessage;
    }
}