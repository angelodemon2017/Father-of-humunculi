using System;

public class ComponentCounter : ComponentData, ISeconder
{
    private int _chanceUpper = 50;
    public bool ChanceUpper => UnityEngine.Random.Range(0, 100) < _chanceUpper;
    private Action _callBack;

    public ComponentCounter(int chanceCall = 50, Action callBack = null)
    {
        _chanceUpper = chanceCall;
        _callBack = callBack;
    }

    public override void DoSecond()
    {
        if (ChanceUpper)
        {
            _callBack?.Invoke();
        }
    }
}