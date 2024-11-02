using System.Diagnostics;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

public class Expiriments : ScriptableObject
{
    static Dictionary<int, object> _objects = new();
    static List<int> _listInts = new();
    static Dictionary<int, int> _ints = new();
    static Dictionary<int, testEnum> _enums = new();
    static int count = 10000000;

    [MenuItem("Tools/MyTool/Do It in C#")]
    static void DoIt()
    {
        for (int i = 0; i < count; i++)
        {
            //            _objects.Add(i, i);
            _listInts.Add(i);
            _ints.Add(i, i);
//            _enums.Add(i, (testEnum)(i % 10));
        }

        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        for (int i = 0; i < count; i++)
        {
            if (_ints.ContainsKey(i))
            {
                _ints[i] = _ints[i] + 1;
            }
//            _objects[i] = (int)_objects[i] + 1;
        }
        stopwatch.Stop();
        UnityEngine.Debug.Log($"ints with check({count}): {stopwatch.ElapsedMilliseconds} ms");

        Stopwatch stopwatch2 = new Stopwatch();
        stopwatch2.Start();
        for (int i = 0; i < count; i++)
        {
            _ints[i] = _ints[i] + 1;
        }
        stopwatch2.Stop();
        UnityEngine.Debug.Log($"ints({count}): {stopwatch2.ElapsedMilliseconds} ms");

        Stopwatch stopwatch3 = new Stopwatch();
        stopwatch3.Start();
        for (int i = 0; i < count; i++)
        {
            _listInts[i]--;
        }
        stopwatch3.Stop();
        UnityEngine.Debug.Log($"_listInts({count}): {stopwatch3.ElapsedMilliseconds} ms");/**/

        Stopwatch stopwatch4 = new Stopwatch();
        stopwatch4.Start();
        for (int i = 0; i < count; i++)
        {
            var asd = _listInts.FirstOrDefault(x => x < i + 1);
            asd++;
        }
        stopwatch4.Stop();
        UnityEngine.Debug.Log($"_listInts with linq({count}): {stopwatch4.ElapsedMilliseconds} ms");
    }
}

public enum testEnum
{
    none,
    enum0,
    enum1,
    enum2,
    enum3,
    enum4,
    enum5,
    enum6,
    enum7,
    enum8,
    enum9,
    enum10,
}