using UnityEngine;

[CreateAssetMenu(menuName = "Components/Item Present Component", order = 1)]
public class ComponentSORectModelForItem : ComponentSO
{
    [SerializeField] private SpriteRenderer _spriteRendererModelPrefab;
    [SerializeField] private ComponentItemPresent _componentData;

    internal override ComponentData GetComponentData => new ComponentItemPresent();

    public override void InitOnScene(EntityMonobeh entityMonobeh)
    {
        var sr = Instantiate(_spriteRendererModelPrefab, entityMonobeh.transform);

        var comp = entityMonobeh.EntityInProcess.EntityData.Components.GetComponent<ComponentItemPresent>();

        if (comp != null)
        {
            sr.sprite = comp.ItemConfig.IconItem;
        }
    }
}