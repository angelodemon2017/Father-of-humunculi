using UnityEngine;

public class ComponentFSM : ComponentData
{
    public string InitState;

    /// <summary>
    /// maybe need change init parametr
    /// </summary>
    /// <param name="initState"></param>
    public ComponentFSM(string initState)
    {
        InitState = initState;
    }

    public override void Init(Transform entityME)
    {
        var ssoc = (FSMController)entityME.gameObject.AddComponent(typeof(FSMController));
        var initState = Resources.Load<State>($"{Config.PathInitStates}/{InitState}");
        ssoc.Init(entityME, initState);
    }
}