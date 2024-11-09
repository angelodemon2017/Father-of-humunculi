using UnityEngine;

[CreateAssetMenu(menuName = "Components/Homu Component", order = 1)]
public class ComponentSOHomu : ComponentSO
{
    [SerializeField] private EnumHomuType _homuType;

    [SerializeField] private ComponentHomu _componentData;

    internal override ComponentData GetComponentData => new ComponentHomu(_homuType);

    public override void InitOnScene(EntityMonobeh entityMonobeh)
    {
        //TODO bad solution =(
        var sr = entityMonobeh.gameObject.GetComponentInChildren<SpriteRenderer>();

        var ch = entityMonobeh.EntityInProcess.EntityData.Components.GetComponent<ComponentHomu>();
        sr.color = ch._colorModelDemo;
    }
}