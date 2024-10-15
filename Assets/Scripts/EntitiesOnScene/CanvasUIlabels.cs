using TMPro;
using UnityEngine;

public class CanvasUIlabels : PrefabByComponentData
{
    [SerializeField] private TextMeshProUGUI _testText;

    private EntityInProcess _entityInProcess;
    private ComponentUIlabels _componentUIlabels;
    private Transform _uicanvas;

    public override string KeyComponent => typeof(ComponentUIlabels).Name;

    public override void Init(ComponentData componentData, EntityInProcess entityInProcess = null)
    {
        _uicanvas = _testText.transform.parent;
        _componentUIlabels = (ComponentUIlabels)componentData;
        _entityInProcess = entityInProcess;

        _entityInProcess.UpdateEIP += UpdateEntity;
        _uicanvas.rotation = Camera.main.transform.rotation;
    }

    private void UpdateEntity()
    {
        _testText.text = _entityInProcess.TestDebugProp;
    }

    private void OnDestroy()
    {
        _entityInProcess.UpdateEIP -= UpdateEntity;
    }
}