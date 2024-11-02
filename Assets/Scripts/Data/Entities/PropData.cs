
using System.Collections.Generic;

public class PropsData
{
    public Dictionary<string, int> Ints = new();

    public int GetInt(string key)
    {
        if (Ints.TryGetValue(key, out int value))
        {
            return value;
        }

        return 0;
    }

    public void SetInt(string key, int value)
    {
        if (Ints.ContainsKey(key))
        {
            Ints[key] = value;
        }
        else
        {
            Ints.Add(key, value);
        }
    }
}