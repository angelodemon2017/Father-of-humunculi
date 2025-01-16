[System.Serializable]
public class Damage
{
    public int InstanseDamage;
    public int RangeDamage;

    public int GetDamage => InstanseDamage + SimpleExtensions.GetRandom(RangeDamage);
}