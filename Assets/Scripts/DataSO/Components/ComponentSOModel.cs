using UnityEngine;

[CreateAssetMenu(menuName = "Components/Model Component", order = 1)]
public class ComponentSOModel : ComponentSO
{
    [SerializeField] private ModelOfEntity _rootOfModel;
    [SerializeField] private ModelController _modelPrefab;
    [SerializeField] private ComponentModelPrefab _componentData;

    internal override ComponentData GetComponentData => new ComponentModelPrefab("");

    public override void InitOnScene(EntityMonobeh entityMonobeh)
    {
        var rootOfModel = Instantiate(_rootOfModel, entityMonobeh.transform);
        rootOfModel.Init(entityMonobeh.EntityInProcess.EntityData.Components.GetComponent<ComponentModelPrefab>(),
            entityMonobeh.EntityInProcess);

        if (_modelPrefab != null)
        {
            rootOfModel.InitModel(_modelPrefab);
        }
    }
}