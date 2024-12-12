using static OptimazeExtensions;

public class ComponentHPData : ComponentData
{
    public int CurrentHP;
    public int RegenHP;//persecond

    public bool IsDeath => CurrentHP <= 0;
    public bool RegenAvailable => RegenHP > 0;

    public ComponentHPData() : base(TypeCache<ComponentHPData>.IdType)
    {

    }
}