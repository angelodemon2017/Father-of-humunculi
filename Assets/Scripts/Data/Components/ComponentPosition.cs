using UnityEngine;
using static OptimazeExtensions;

public class ComponentPosition : ComponentData
{
    public float Xpos;
    public float Zpos;

    public UnityEngine.Vector3 Position => new UnityEngine.Vector3(Xpos, 0f, Zpos);

    public ComponentPosition(float xpos, float zpos) : base(TypeCache<ComponentPosition>.IdType)
    {
        Xpos = xpos;
        Zpos = zpos;
    }

    public void UpdateByCommand(string argument)
    {
        var coords = argument.Split('|');
        Xpos = float.Parse(coords[0]);
        Zpos = float.Parse(coords[1]);
    }

    public static CommandData CommandUpdate(Vector3 position)
    {//TODO Need think about this solution
        return new CommandData(-1, TypeCache<ComponentPosition>.IdType, $"{position.x}|{position.z}");
    }

    public static string CommandArgumentUpdate(Vector3 position)
    {
        return $"{position.x},{position.z}";
    }
}