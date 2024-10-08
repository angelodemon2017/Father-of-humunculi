using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ListExtensions
{
    public static T GetRandom<T>(this IEnumerable<T> list)
    {
        return list.ElementAt(Random.Range(0, list.Count()));
    }

    public static T GetRandom<T>(this IEnumerable<T> list, int swift)
    {
        var needIndex = swift % list.Count();

        return list.ElementAt(needIndex);
    }

    public static EnumTileDirect GetSummaryMask(this List<EnumTileDirect> directs)
    {
        EnumTileDirect result = EnumTileDirect.none;

        directs.ForEach(d => result |= d);

        return result;
    }
}