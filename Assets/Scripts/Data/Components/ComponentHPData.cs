public class ComponentHPData : ComponentData
{
    public int CurrentHP;
    public int RegenHP;//persecond

    public bool IsDeath => CurrentHP <= 0;
    public bool RegenAvailable => RegenHP > 0;

    public ComponentHPData()
    {

    }

    public ComponentHPData(int maxHP, int regenHP)
    {
        CurrentHP = maxHP;
        RegenHP = regenHP;
    }
}