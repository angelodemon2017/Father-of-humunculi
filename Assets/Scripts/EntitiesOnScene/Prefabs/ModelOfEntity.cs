using UnityEngine;

public class ModelOfEntity : PrefabByComponentData
{
    [SerializeField] private GameObject _shadow;

    private ModelController _modelController;
//    private Transform _model;
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

        var model = Instantiate(go, transform.position + go.transform.position, CameraController.Instance.DirectParalCamera, transform).transform;
        _modelController = model.GetComponent<ModelController>();

        Instantiate(_shadow, transform.position + Vector3.up * 0.01f, Quaternion.identity, transform);

        UpdateModel();
    }

    private void UpdateModel()
    {
        _modelController?.SomeCheck(_componentModelPrefab.CurrentParamOfModel);
    }

    private void OnDestroy()
    {
        if (_entityInProcess != null)
        {
            _entityInProcess.UpdateEIP -= UpdateModel;
        }
    }
}