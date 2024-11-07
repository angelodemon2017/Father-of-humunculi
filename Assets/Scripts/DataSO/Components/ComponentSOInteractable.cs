using UnityEngine;

[CreateAssetMenu(menuName = "Components/Interactable Component", order = 1)]
public class ComponentSOInteractable : ComponentSO
{
    [SerializeField] private MouseInterfaceInteraction _interactablePrefab;
    [SerializeField] private ComponentInterractable _componentData;

    internal override ComponentData GetComponentData => new ComponentInterractable(GetString);

    private string GetString() => "pickUp";

    public override void InitOnScene(EntityMonobeh entityMonobeh)
    {
        var interactableComponent = Instantiate(_interactablePrefab, entityMonobeh.transform);
        interactableComponent.Init(entityMonobeh,
            entityMonobeh.EntityInProcess.EntityData.Components.GetComponent<ComponentInterractable>());
    }
}