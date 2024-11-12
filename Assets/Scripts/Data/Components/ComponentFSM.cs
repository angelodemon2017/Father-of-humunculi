public class ComponentFSM : ComponentData
{
    public State StartState;

    public ComponentFSM()
    {

    }

    public ComponentFSM(State initState)
    {
        StartState = initState;
    }
}