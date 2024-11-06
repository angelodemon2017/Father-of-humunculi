using UnityEngine;

[CreateAssetMenu(menuName = "Components/UI label Component", order = 1)]
public class ComponentSOUILabels : ComponentSO
{
    [SerializeField] private CanvasUIlabels _labelPrefab;
    [SerializeField] private Color _textColor;
    [SerializeField] private float _hightText;
    [SerializeField] private string _defaultText;
    [SerializeField] private ComponentUIlabels _componentData;

    internal override ComponentData GetComponentData => new ComponentUIlabels(_textColor, _hightText, _defaultText);

    public override void InitOnScene(EntityMonobeh entityMonobeh)
    {
        var uiLabel = Instantiate(_labelPrefab, entityMonobeh.transform);
        uiLabel.Init(entityMonobeh.EntityInProcess.EntityData.Components.GetComponent<ComponentUIlabels>(),
            entityMonobeh.EntityInProcess);
    }
}