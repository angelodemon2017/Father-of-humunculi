using System;

[Serializable]
public class ComponentCounter : ComponentData//, ISeconder
{
    public int _chanceUpper = 50;
    public int _debugCounter = 0;
    public int _maxCount = 0;

    public ComponentCounter(ComponentCounter component)
    {
        _chanceUpper = component._chanceUpper;
        _debugCounter = component._debugCounter;
        _maxCount = component._maxCount;
    }

    public ComponentCounter(int chanceCall = 50)
    {
        _chanceUpper = chanceCall;
    }

/*    public override bool DoSecond()
    {
        if ((_maxCount == 0 || _debugCounter < _maxCount) &&
            _chanceUpper.GetChance())
        {
            _debugCounter++;
            return true;
        }
        return false;
    }/**/
}