using static OptimazeExtensions;

public class ComponentHPData : ComponentData
{
    public int CurrentHP;
    public int RegenHP;//persecond
    public int MaxHP;
    public int TimeoutRegen;

    public bool IsDeath => CurrentHP <= 0;
    public bool RegenAvailable => RegenHP > 0;

    public ComponentHPData(int maxHP, int regenHP) : base(TypeCache<ComponentHPData>.IdType)
    {
        RegenHP = regenHP;
        CurrentHP = maxHP;
        MaxHP = maxHP;
    }
}