using static OptimazeExtensions;

public class ComponentFSM : ComponentData
{
    public string CurrentState;
    public long EntityOfBirth;
    public long EntityTarget = -1;
    public int xPosFocus;
    public int zPosFocus;

    public ComponentFSM(string initState) : base(TypeCache<ComponentFSM>.IdType)
    {
        CurrentState = initState;
    }

    public void SetPosFocus(float x, float z)
    {
        xPosFocus = (int)x;
        zPosFocus = (int)z;
    }
}