using UnityEngine;

public static class SimpleExtensions
{
    public static float FixIndex(this float index, int count)
    {
        if (index >= count)
        {
            index = count - 1;
        }
        if (index < 0)
        {
            index = 0;
        }
        return index;
    }

    public static bool GetChance(this int chance)
    {
        return Random.Range(0, 100) < chance;
    }
}