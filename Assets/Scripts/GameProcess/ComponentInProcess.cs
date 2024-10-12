public class ComponentInProcess<T>
    where T : ComponentData
{
    protected T _dataComponent;

    public ComponentInProcess(T dataComponent)
    {
        _dataComponent = dataComponent;
    }

    public virtual void DoSecond()
    {
        _dataComponent.DoSecond();
    }
}