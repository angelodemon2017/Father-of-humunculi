using UnityEngine;

public class ModelOfEntity : PrefabByComponentData
{
    private const string pathModels = "EntityModels";

    public Transform _model;
    private ComponentModelPrefab _componentUIlabels;
    private EntityInProcess _entityInProcess;

    public override string KeyComponent => typeof(ComponentModelPrefab).Name;

    public override void Init(ComponentData componentData, EntityInProcess entityInProcess = null)
    {
        _componentUIlabels = (ComponentModelPrefab)componentData;

        _entityInProcess = entityInProcess;
        _entityInProcess.UpdateEIP += UpdateModel;

        InitModel();
    }

    private void UpdateModel()
    {
        _model.localRotation = CameraController.Instance.DirectParalCamera;
    }

    private void InitModel()
    {
        transform.DestroyChildrens();

        var go = Resources.Load<GameObject>($"{pathModels}/{_componentUIlabels.KeyModel}");

        if (go == null)
        {
            return;
        }

        _model = Instantiate(go, transform.position + go.transform.position, CameraController.Instance.DirectParalCamera, transform).transform;
    }

    private void OnDestroy()
    {
        _entityInProcess.UpdateEIP -= UpdateModel;
    }
}