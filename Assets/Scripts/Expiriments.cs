using System.Diagnostics;
using UnityEditor;
using UnityEngine;
using System;

public class Expiriments : ScriptableObject
{
    static int count = 10000000;

    [MenuItem("Tools/MyTool/Do It in C#")]
    static void DoIt()
    {
        for (int i = 0; i < count; i++)
        {

        }

        //        RunFunc(TestFunc1, "just");
    }

    static private void RunFunc(Action action, string comm)
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        action.Invoke();
        stopwatch.Stop();
        UnityEngine.Debug.Log($"{comm}: {stopwatch.ElapsedMilliseconds} ms");
    }
}