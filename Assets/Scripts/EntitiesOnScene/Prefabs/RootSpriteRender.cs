using UnityEngine;
using static OptimazeExtensions;

public class RootSpriteRender : PrefabByComponentData
{
    [SerializeField] private SpriteRenderer _spriteRenderer;

    public override int KeyComponentData => TypeCache<ComponentModelPrefab>.IdType;

    internal override ComponentData GetComponentData => new ComponentModelPrefab();

    public override void Init(ComponentData componentData, EntityInProcess entityInProcess = null)
    {

    }

    public void SetSprite(Sprite sprite)
    {
        _spriteRenderer.sprite = sprite;
    }
}