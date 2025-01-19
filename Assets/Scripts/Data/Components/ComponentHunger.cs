using System.Collections.Generic;
using System.Linq;
using static OptimazeExtensions;

public class ComponentHunger : ComponentData
{
    public int Starvation;
    public int MaxStarvation;
    public int Saturation;

    private Dictionary<string, int> _gorging = new();
    public const int Gorgbad = 50;
    public const int Gorgfatal = 100;

    internal List<KeyValuePair<string,int>> Gorging => _gorging.ToList();

    public ComponentHunger(int maxStarv) : base(TypeCache<ComponentHunger>.IdType) 
    {
        MaxStarvation = maxStarv;
        Starvation = maxStarv;
        Saturation = 1;
    }

    public void RestoreHunger(int starvation, int saturation, string itemKey, int gorg)
    {
        var curGorg = GetGorg(itemKey);
        if (curGorg > Gorgfatal)
        {
            starvation = 0;
        }
        else if (curGorg > Gorgbad)
        {
            starvation /= 2;
        }

        Starvation += starvation;
        if (Starvation > MaxStarvation)
        {
            Starvation = MaxStarvation;
        }
        if (Saturation < saturation)
        {
            Saturation = saturation;
        }
        AddGorging(itemKey, gorg);
    }

    private int GetGorg(string itemKey)
    {
        if (_gorging.TryGetValue(itemKey, out int val))
        {
            return val;
        }

        return 0;
    }

    private void AddGorging(string itemKey, int gorg)
    {
        if (!_gorging.ContainsKey(itemKey))
        {
            _gorging.Add(itemKey, gorg);
        }
        else
        {
            _gorging[itemKey] += gorg;
        }
    }

    HashSet<string> forDel = new();
    internal void CalcGorgingBySecond()
    {
        var tempGorg = _gorging.ToList();
        foreach (var g in tempGorg)
        {
            _gorging[g.Key]--;
            if (_gorging[g.Key] <= 0)
            {
                forDel.Add(g.Key);
            }
        }
        foreach (var key in forDel)
        {
            _gorging.Remove(key);
        }
        forDel.Clear();
    }

    public bool ApplyHunger(int hunger)
    {
        if (Saturation > 0)
        {
            Saturation -= hunger;
            if (Saturation < 0)
            {
                Saturation = 0;
            }
        }
        else if(Starvation > 0)
        {
            Starvation -= hunger;
            if (Starvation < 0)
            {
                Starvation = 0;
            }
        }
        else
        {
            return true;
        }

        return false;
    }
}