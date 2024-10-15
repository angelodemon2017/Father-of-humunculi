using System;

public abstract class ComponentData
{
    public string KeyName => GetType().Name;
    public Action changed;

    public virtual void DoSecond()
    {

    }
}

public class ComponentPosition : ComponentData
{
    public float Xpos;
    public float Zpos;

    public UnityEngine.Vector3 Position => new UnityEngine.Vector3(Xpos, 0f, Zpos);

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
    public string ComponentKey;
    public string ContainerContent;
}

public class ComponentCounter : ComponentData, ISeconder
{
    private int _chanceUpper = 50;
    public bool ChanceUpper => UnityEngine.Random.Range(0, 100) < _chanceUpper;
    private Action _callBack;

    public ComponentCounter(int chanceCall = 50, Action callBack = null)
    {
        _chanceUpper = chanceCall;
        _callBack = callBack;
    }

    public override void DoSecond()
    {
        if (ChanceUpper)
        {
            _callBack?.Invoke();
        }
    }
}

public class ComponentUIlabels : ComponentData
{

}

public class ComponentModelPrefab : ComponentData
{
    public string KeyModel;

    public ComponentModelPrefab(string keyModel) : base()
    {
        KeyModel = keyModel;
    }
}

public class ComponentInterractable : ComponentData
{

}