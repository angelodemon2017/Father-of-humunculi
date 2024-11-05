using UnityEngine;

[CreateAssetMenu(menuName = "Components/Counter Component", order = 1)]
public class ComponentSOCounter : ComponentSO, ISeconderEntity
{
    [SerializeField] private ComponentCounter _defaultComponentData;

    internal override ComponentData GetComponentData => new ComponentCounter(_defaultComponentData);

    public void DoSecond(EntityData entity)
    {
        var comp = entity.Components.GetComponent<ComponentCounter>();

        if (comp._chanceUpper.GetChance())
        {
            comp._debugCounter++;
            var cmp = entity.Components.GetComponent<ComponentModelPrefab>();
            cmp.CurrentParamOfModel = comp._debugCounter;
            entity.UpdateEntity();
        }
    }
}