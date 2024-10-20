using UnityEngine;

public class ItemRoot : PrefabByComponentData
{
    [SerializeField] private SpriteRenderer _spriteRenderer;

    private Transform _model;
    private ComponentItemPresent _componentItemPresent;
    private EntityInProcess _entityInProcess;

    public override string KeyComponent => typeof(ComponentItemPresent).Name;

    public override void Init(ComponentData componentData, EntityInProcess entityInProcess = null)
    {
        _model = _spriteRenderer.transform;
        _componentItemPresent = (ComponentItemPresent)componentData;

        _entityInProcess = entityInProcess;
        _entityInProcess.UpdateEIP += UpdateModel;

        _spriteRenderer.sprite = _componentItemPresent.ItemConfig.IconItem;

        UpdateModel();
    }

    private void UpdateModel()
    {
        _model.rotation = CameraController.Instance.DirectParalCamera;
    }

    private void OnDestroy()
    {
        if (_entityInProcess != null)
        {
            _entityInProcess.UpdateEIP -= UpdateModel;
        }
    }
}