using System;

public abstract class ComponentData
{
    public Action changed;
}

public class ComponentPosition : ComponentData
{
    public float Xpos;
    public float Zpos;



    public ComponentPosition(float xpos, float zpos)
    {
        Xpos = xpos;
        Zpos = zpos;
    }
}

public class ComponentPlayerId : ComponentData
{
    public string PlayerName;
}

public class ComponentHPData : ComponentData
{
    public int CurrentHP;
    public int MaxHP;
    public int RegenHP;//persecond

    public bool IsDeath => CurrentHP <= 0;
    public bool RegenAvailable => RegenHP > 0;
}

public class ComponentContainerData
{
    public string ComponentName;
    public string ContainerContent;
}