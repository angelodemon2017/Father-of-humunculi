using System;

[Serializable]
public class ComponentCounter : ComponentData//, ISeconder
{
    public int _chanceUpper = 50;
    public int _debugCounter = 0;
    public int _maxCount = 0;

    public ComponentCounter(ComponentCounter component)
    {
        AddingKey = component.AddingKey;
        _chanceUpper = component._chanceUpper;
        _debugCounter = component._debugCounter;
        _maxCount = component._maxCount;
    }

    public ComponentCounter(int chanceCall = 50)
    {
        _chanceUpper = chanceCall;
    }
}