public static class EnumExtensions
{
    public static EntityData GetEntityByRecipe(this EnumBuilds build, float x, float z)
    {
        switch (build)
        {
            case EnumBuilds.DebugShop:
                return new EntityShop(x, z);
            case EnumBuilds.DemoFabric:
                return new EntityDemoFabric(x, z);
            default:
                return new EntityData();
        }
    }
}