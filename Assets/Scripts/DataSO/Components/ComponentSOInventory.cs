using UnityEngine;

[CreateAssetMenu(menuName = "Components/Inventory Component", order = 1)]
public class ComponentSOInventory : ComponentSO
{
//    [SerializeField] private int _maxItems = 5;
    [SerializeField] private ComponentInventory _componentData;

    internal override ComponentData GetComponentData => new ComponentInventory(_componentData);

    public override void InitOnScene(EntityMonobeh entityMonobeh)
    {
        entityMonobeh.EntityInProcess.EntityData.Components.GetComponent<ComponentInventory>().Init(entityMonobeh.transform);
    }
}