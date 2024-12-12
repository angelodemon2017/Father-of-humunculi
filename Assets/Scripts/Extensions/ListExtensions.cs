using System.Collections.Generic;
using System.Linq;

public static class ListExtensions
{
    public static T GetRandom<T>(this IEnumerable<T> list)
    {
        return list.ElementAt(list.Count().GetRandom());
    }

    public static T GetRandom<T>(this IEnumerable<T> list, int swift)
    {
        var needIndex = swift % list.Count();

        if (needIndex < 0 || needIndex > list.Count())
        {
            return list.GetRandom();
        }

        return list.ElementAt(needIndex);
    }

    public static EnumTileDirect GetSummaryMask(this List<EnumTileDirect> directs)
    {
        EnumTileDirect result = EnumTileDirect.none;

        directs.ForEach(d => result |= d);

        return result;
    }

    public static List<PrefabByComponentData> GetComponents(this List<PrefabByComponentData> components, ComponentData componentData)
    {//TODO need think about lack that method...
        return components.Where(x => x.KeyComponentData == componentData.KeyType && x.AddingKey == componentData.AddingKey).ToList();
    }
}