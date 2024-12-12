using System;
using static OptimazeExtensions;

[Serializable]
public class ComponentCounter : ComponentData//, ISeconder
{
    public int _chanceUpper = 50;
    public int _debugCounter = 0;
    public int _maxCount = 0;

    public ComponentCounter(ComponentCounter component) : base(TypeCache<ComponentCounter>.IdType)
    {
        AddingKey = component.AddingKey;
        _chanceUpper = component._chanceUpper;
        _debugCounter = component._debugCounter;
        _maxCount = component._maxCount;
    }

    public ComponentCounter(int chanceCall = 50) : base(TypeCache<ComponentCounter>.IdType)
    {
        _chanceUpper = chanceCall;
    }
}