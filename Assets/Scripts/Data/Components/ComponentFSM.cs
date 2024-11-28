public class ComponentFSM : ComponentData
{
    public string CurrentState;
    public long EntityOfBirth;
    public long EntityTarget;
    public int xPosFocus;
    public int zPosFocus;

    public ComponentFSM()
    {

    }

    public ComponentFSM(string initState)
    {
        CurrentState = initState;
    }

    public void SetPosFocus(float x, float z)
    {
        xPosFocus = (int)x;
        zPosFocus = (int)z;
    }
}