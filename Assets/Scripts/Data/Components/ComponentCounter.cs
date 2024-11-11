using System;

[Serializable]
public class ComponentCounter : ComponentData//, ISeconder
{
    public int _chanceUpper = 50;
    public int _debugCounter = 0;
    private Action _callBack;

    public ComponentCounter(ComponentCounter component)
    {
        _chanceUpper = component._chanceUpper;
        _debugCounter = component._debugCounter;
    }

    public ComponentCounter(int chanceCall = 50, Action callBack = null)
    {
        _chanceUpper = chanceCall;
        _callBack = callBack;
    }

    public override bool DoSecond()
    {
        if (_chanceUpper.GetChance())
        {
            _debugCounter++;
            return true;
//            _callBack?.Invoke();
        }
        return false;
    }
}