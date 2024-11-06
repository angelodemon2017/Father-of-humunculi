public class ComponentFSM : ComponentData
{
    public State StartState;

    public ComponentFSM(State initState)
    {
        StartState = initState;
    }
}