public class ComponentInProcess<T>
{
    protected T _dataComponent;

    public void Init(T dataComponent)
    {
        _dataComponent = dataComponent;
    }

    public virtual void Second()
    {

    }
}