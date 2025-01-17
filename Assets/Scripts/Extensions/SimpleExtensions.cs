﻿using System;

public static class SimpleExtensions
{
    private static Random rnd = new();

    public static float FixMinMax(this float index, float min, float max)
    {
        if (index >= max)
        {
            index = max;
        }
        if (index < min)
        {
            index = min;
        }
        return index;
    }

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
        return rnd.Next(100) < chance;
    }

    public static int GetRandom(this int max)
    {
        return rnd.Next(max);
    }

    public static float GetRandom(float min, float max)
    {
        return ((float)rnd.Next((int)(min * 1000), (int)(max * 1000))) / 1000f;
    }

    public static string SimpleFormat(this float val)
    {
        return val.ToString("#.##");
    }
}