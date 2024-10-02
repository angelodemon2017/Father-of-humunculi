public class ComponentHPInProcess : ComponentInProcess<ComponentHPData>
{
    public override void Second()
    {
        if (_dataComponent.CurrentHP >= _dataComponent.MaxHP)
            return;


    }
}