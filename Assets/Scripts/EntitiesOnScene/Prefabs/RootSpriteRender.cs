using UnityEngine;

public class RootSpriteRender : PrefabByComponentData
{//TODO DELETE AFTER EXP
    [SerializeField] private SpriteRenderer _spriteRenderer;

//    private ComponentItemPresent _componentItemPresent;
//    private EntityInProcess _entityInProcess;

    public override string KeyComponent => typeof(RootSpriteRender).Name;
    public override string KeyComponentData => typeof(ComponentModelPrefab).Name;

    internal override ComponentData GetComponentData => new ComponentModelPrefab();

    public override void Init(ComponentData componentData, EntityInProcess entityInProcess = null)
    {
//        _componentItemPresent = (ComponentItemPresent)componentData;

//        _entityInProcess = entityInProcess;
//        _entityInProcess.UpdateEIP += UpdateModel;

//        _spriteRenderer.sprite = _componentItemPresent.ItemConfig.IconItem;

//        UpdateModel();
    }

    public void SetSprite(Sprite sprite)
    {
        _spriteRenderer.sprite = sprite;
    }

    private void OnDestroy()
    {
/*        if (_entityInProcess != null)
        {
            _entityInProcess.UpdateEIP -= UpdateModel;
        }/**/
    }
}