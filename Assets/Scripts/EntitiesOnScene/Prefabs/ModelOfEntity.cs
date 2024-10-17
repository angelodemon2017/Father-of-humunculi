using UnityEngine;

public class ModelOfEntity : PrefabByComponentData
{
    private ModelController _modelController;
    private Transform _model;
    private ComponentModelPrefab _componentModelPrefab;
    private EntityInProcess _entityInProcess;

    public override string KeyComponent => typeof(ComponentModelPrefab).Name;

    public override void Init(ComponentData componentData, EntityInProcess entityInProcess = null)
    {
        _componentModelPrefab = (ComponentModelPrefab)componentData;

        _entityInProcess = entityInProcess;
        _entityInProcess.UpdateEIP += UpdateModel;

        InitModel();
    }

    private void InitModel()
    {
        transform.DestroyChildrens();

        var go = Resources.Load<GameObject>($"{Config.PathEntityModels}/{_componentModelPrefab.KeyModel}");

        if (go == null)
        {
            return;
        }

        _model = Instantiate(go, transform.position + go.transform.position, CameraController.Instance.DirectParalCamera, transform).transform;
        _modelController = _model.GetComponent<ModelController>();

        UpdateModel();
    }

    private void UpdateModel()
    {
        _model.localRotation = CameraController.Instance.DirectParalCamera;

        _modelController?.SomeCheck(_componentModelPrefab.CurrentParamOfModel);
    }

    private void OnDestroy()
    {
        _entityInProcess.UpdateEIP -= UpdateModel;
    }
}