using UnityEngine;

[CreateAssetMenu(menuName = "Components/Rect Model Component", order = 1)]
public class ComponentSORectModel : ComponentSO
{
    [SerializeField] private Sprite _spriteModel;
    [SerializeField] private SpriteRenderer _spriteRendererModelPrefab;
    [SerializeField] private ComponentModelByRectPrefab _componentData;

    internal override ComponentData GetComponentData => new ComponentModelByRectPrefab();

    public override void InitOnScene(EntityMonobeh entityMonobeh)
    {
        var sr = Instantiate(_spriteRendererModelPrefab, entityMonobeh.transform);

        sr.sprite = _spriteModel;
    }
}