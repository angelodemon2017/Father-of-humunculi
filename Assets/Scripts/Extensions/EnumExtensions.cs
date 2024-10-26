public static class EnumExtensions
{
    public static EntityData GetEntityByRecipe(this EnumBuilds build, float x, float z)
    {
        switch (build)
        {
            case EnumBuilds.DebugShop:
                return new EntityShop(x, z);
            default:
                return new EntityData();
        }
    }
}