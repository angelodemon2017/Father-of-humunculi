using UnityEngine;

[CreateAssetMenu(menuName = "Components/UI panel Component", order = 1)]
public class ComponentSOShovedUIPanel : ComponentSO
{
    [SerializeField] private PrefabByComponentData _UIprefab;
    [SerializeField] private ComponentUICraftGroup _componentData;
    [SerializeField] private GroupSO _recipeGroup;

    internal override ComponentData GetComponentData => new ComponentUICraftGroup(_recipeGroup.GroupName);

    public override void InitOnScene(EntityMonobeh entityMonobeh)
    {
        var uiPref = Instantiate(_UIprefab, entityMonobeh.transform);

        var cmp = entityMonobeh.EntityInProcess.EntityData.Components.GetComponent<ComponentUICraftGroup>();

        uiPref.Init(cmp, entityMonobeh.EntityInProcess);
    }
}