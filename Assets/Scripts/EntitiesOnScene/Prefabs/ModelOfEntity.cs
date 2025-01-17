using UnityEngine;
using static OptimazeExtensions;

public class ModelOfEntity : PrefabByComponentData
{
    public override int KeyType => TypeCache<ModelOfEntity>.IdType;
    [SerializeField] private GameObject _shadow;

    private ModelController _modelController;
//    private Transform _model;
    private ComponentModelPrefab _componentModelPrefab;
    private EntityInProcess _entityInProcess;

    public override void Init(ComponentData componentData, EntityInProcess entityInProcess = null)
    {
        _componentModelPrefab = (ComponentModelPrefab)componentData;

        _entityInProcess = entityInProcess;
        _entityInProcess.UpdateEIP += UpdateModel;

        InitModel();
    }

    public void InitModel(ModelController modelController)
    {
        transform.DestroyChildrens();

        _modelController = Instantiate(modelController, transform.position + modelController.transform.position, CameraController.Instance.DirectParalCamera, transform);

        Instantiate(_shadow, transform.position + Vector3.up * 0.01f, Quaternion.identity, transform);

        UpdateModel();
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