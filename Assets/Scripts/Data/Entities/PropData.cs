
using System.Collections.Generic;

public class PropsData
{
    public float xpos;
    public float zpos;

    /*    public DictProp<int> Ints = new();
        public DictProp<float> Floats = new();
        public DictProp<string> Texts = new();
        public DictProp<ItemData> Items = new();/**/
}

public class DictProp<T>//?? : Dictionary<string, T>
{
    public Dictionary<string, T> Props = new();

    public void AddProp(string key)
    {
        Props.Add(key, default(T));
    }

    public T GetProp(string key)
    {
        return Props[key];
/*        if (Props.TryGetValue(key, out T value))
        {
            return value;
        }

        return default(T);/**/
    }

    public void SetProp(string key, T value)
    {
        Props[key] = value;
    }
}