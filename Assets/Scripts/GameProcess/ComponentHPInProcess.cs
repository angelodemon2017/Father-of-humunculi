public class ComponentHPInProcess : ComponentInProcess<ComponentHPData>
{
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

        Restore(_dataComponent.RegenHP);
    }
}