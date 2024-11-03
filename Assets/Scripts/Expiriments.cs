using System.Diagnostics;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

public class Expiriments : ScriptableObject
{
    static Dictionary<int, object> _objects = new();
    static Dictionary<int, object> _ints = new();
    static Dictionary<int, SimpleClass> simpls = new();
    static Dictionary<string, SimpleClass> simpls2 = new();
    static int count = 10000000;
    static int justInt = 0;

    [MenuItem("Tools/MyTool/Do It in C#")]
    static void DoIt()
    {
        _ints.Clear();
        _objects.Clear();
        simpls.Clear();
        simpls2.Clear();
        justInt = 0;

        for (int i = 0; i < count; i++)
        {
            _ints.Add(i, new SimpleClass());
            _objects.Add(i, new HeavyClass());
            simpls.Add(i, new SimpleClass());
        }
        simpls2.Add("key", new SimpleClass());

        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        for (int i = 0; i < count; i++)
        {
            SimpleClass val = (SimpleClass)_ints[i];
            val.TARGETFIELD += 1;
            _ints[i] = val;
        }
        stopwatch.Stop();
        UnityEngine.Debug.Log($"object(int)({count}): {stopwatch.ElapsedMilliseconds} ms");

        Stopwatch stopwatch2 = new Stopwatch();
        stopwatch2.Start();
        for (int i = 0; i < count; i++)
        {
            justInt++;
        }
        stopwatch2.Stop();
        UnityEngine.Debug.Log($"justInt({count}): {stopwatch2.ElapsedMilliseconds} ms");

        Stopwatch stopwatch3 = new Stopwatch();
        stopwatch3.Start();
        for (int i = 0; i < count; i++)
        {
            simpls[1].TARGETFIELD += 1;
        }
        stopwatch3.Stop();
        UnityEngine.Debug.Log($"object(simples[int])({count}): {stopwatch3.ElapsedMilliseconds} ms");

        Stopwatch stopwatch4 = new Stopwatch();
        stopwatch4.Start();
        for (int i = 0; i < count; i++)
        {
            simpls2["key"].TARGETFIELD += 1;
        }
        stopwatch4.Stop();
        UnityEngine.Debug.Log($"object(simples[string])({count}): {stopwatch4.ElapsedMilliseconds} ms");
    }
}

public class HeavyClass
{
    public HeavyClass2 heavyClass2 = new();
    public float someField;
    private string someText;
    public List<ItemData> items = new();

    public class HeavyClass2
    {
        public HeavyClass3 heavyClass3 = new();
        public float someField;
        private string someText;
        public List<ItemData> items = new();
    }
}

public class HeavyClass3
{
    public int TARGETFIELD;
    public float someField;
    private string someText;
}

public class SimpleClass
{
    public int TARGETFIELD;
}