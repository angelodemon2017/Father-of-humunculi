using System.Collections.Generic;

public class CashDictionary<TKey, TVal> where TVal : IDictKey<TKey>
{
    private Dictionary<TKey, List<TVal>> _cashDict = new();

    public void AddElement(TVal val)
    {
        var tempKey = val.GetKey();
        if (!_cashDict.ContainsKey(tempKey))
        {
            _cashDict.Add(tempKey, new List<TVal>());
        }

        _cashDict[tempKey].Add(val);
    }

    internal void RemoveElement(TVal val)
    {
        var tempKey = val.GetKey();
        _cashDict[tempKey].Remove(val);
    }

    internal bool TryGetValue(TKey key, out List<TVal> result)
    {
        result = new List<TVal>();
        if (_cashDict.TryGetValue(key, out List<TVal> values))
        {
            result.AddRange(values);
            return true;
        }
        else
        {
            return false;
        }
    }
}