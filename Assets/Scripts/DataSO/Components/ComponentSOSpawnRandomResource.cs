using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Components/Spawn Random Item Component", order = 1)]
public class ComponentSOSpawnRandomResource : ComponentSO
{
    [SerializeField] private List<ItemConfig> _items;
    [SerializeField] private SpriteRenderer _spriteRendererModelPrefab;
    [SerializeField] private ComponentItemPresent _componentData;

    internal override ComponentData GetComponentData => new ComponentItemPresent(new ItemData(_items.GetRandom()));

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