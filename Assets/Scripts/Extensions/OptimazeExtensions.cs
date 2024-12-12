public static class OptimazeExtensions
{
    public static class PoolCounter<T>
    {
        private static int _counter = 0;
        public static int NextUid()
        {
            _counter++;
            return _counter;
        }
    }

    private static int _idCounter = 0;
    public static int GetNextId()
    {
        _idCounter++;
        return _idCounter;
    }
    public static class TypeCache<T>
    {
        public static readonly int IdType = GetNextId();
    }
}