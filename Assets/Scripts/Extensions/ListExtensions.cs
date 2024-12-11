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

    public static List<ComponentData> GetComponents(this List<ComponentData> components, string keyName)
    {
        return components.Where(x => x.KeyName == keyName).ToList();
    }

    public static T GetComponent<T>(this List<ComponentData> components, string addKey = "") where T : ComponentData
    {
        return components.FirstOrDefault(x => x.KeyName == typeof(T).Name && 
            (string.IsNullOrEmpty(addKey) || x.AddingKey == addKey)) as T;
    }

    public static T GetComponent<T>(this List<PrefabByComponentData> components) where T : PrefabByComponentData
    {
        return components.FirstOrDefault(x => x.KeyComponent == typeof(T).Name) as T;
    }

    public static List<PrefabByComponentData> GetComponents(this List<PrefabByComponentData> components, ComponentData componentData)
    {
        return components.Where(x => x.KeyComponentData == componentData.KeyName && x.AddingKey == componentData.AddingKey).ToList();
    }
}