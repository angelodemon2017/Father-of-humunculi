public class ComponentFSM : ComponentData
{
    public long EntityOfBirth;
    public long EntityTarget;
    public int xPosFocus;
    public int zPosFocus;

    public ComponentFSM()
    {

    }

    public void SetPosFocus(float x, float z)
    {
        xPosFocus = (int)x;
        zPosFocus = (int)z;
    }
}