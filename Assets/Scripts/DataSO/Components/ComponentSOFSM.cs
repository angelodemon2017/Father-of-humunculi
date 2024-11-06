using UnityEngine;

[CreateAssetMenu(menuName = "Components/FSM Component", order = 1)]
public class ComponentSOFSM : ComponentSO
{
    [SerializeField] private State _startState;
//    [SerializeField] private ComponentFSM _componentData;

    internal override ComponentData GetComponentData => new ComponentFSM(_startState);

    public override void InitOnScene(EntityMonobeh entityMonobeh)
    {
        var ssoc = (FSMController)entityMonobeh.gameObject.AddComponent(typeof(FSMController));
        var startState = entityMonobeh.EntityInProcess.EntityData.Components.GetComponent<ComponentFSM>().StartState;
        ssoc.Init(entityMonobeh.transform, startState);
    }
}