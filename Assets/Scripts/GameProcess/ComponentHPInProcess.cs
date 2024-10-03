public class ComponentHPInProcess : ComponentInProcess<ComponentHPData>
{
    const int TIME_WAIT = 3;
    private int _timerWaitRegen = 0;

    public ComponentHPInProcess(ComponentHPData component) : base(component) { }

    public override void DoSecond()
    {
        Regeneration();
    }

    public void TakeDamage(int amount)
    {
        _dataComponent.CurrentHP -= amount;
        if (_dataComponent.CurrentHP < 0)
        {
            _dataComponent.CurrentHP = 0;
        }

        _timerWaitRegen = TIME_WAIT;
    }

    public void Restore(int amount)
    {
        _dataComponent.CurrentHP += amount;
        if (_dataComponent.CurrentHP > _dataComponent.MaxHP)
        {
            _dataComponent.CurrentHP = _dataComponent.MaxHP;
        }
    }

    private void Regeneration()
    {
        if (_dataComponent.IsDeath || 
            _dataComponent.CurrentHP >= _dataComponent.MaxHP || 
            !_dataComponent.RegenAvailable)
            return;

        if (_timerWaitRegen > 0)
        {
            _timerWaitRegen--;
            return;
        }

        Restore(_dataComponent.RegenHP);
    }
}