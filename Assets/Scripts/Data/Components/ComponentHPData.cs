public class ComponentHPData : ComponentData
{
    public int CurrentHP;
    public int MaxHP;
    public int RegenHP;//persecond

    public bool IsDeath => CurrentHP <= 0;
    public bool RegenAvailable => RegenHP > 0;

    public ComponentHPData(int maxHP, int regenHP)
    {
        CurrentHP = maxHP;
        MaxHP = maxHP;
        RegenHP = regenHP;
    }
}