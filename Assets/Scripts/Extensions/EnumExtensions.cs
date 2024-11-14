public static class EnumExtensions
{
    public static bool CheckAction(this EnumControlInputPlayer controlInputPlayer, bool isStop = false)
    {
        return GameplayClient.Instance.CheckAction(controlInputPlayer, isStop);
    }
}