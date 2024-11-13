public static class Config
{
    public static int ChunkTilesSize = 3;
    public static float TileSize = 2f;
    public static int VisibilityChunkDistance = 4;
    public static int EntitiesInChunk = 1;

    public static float CloseUIDistance = 2f;
    public static float DistanceForShowHeyHint = 2f;

    public static string PathEntityModels = "EntityModels";
    public static string PathEntityConfigs = "Entities/EntityConfigs";
    public static string PathComponents = "Entities/Components";
    public static string PathECFBs = "Entities/ECFBs";
    public static string PathPrefabsByComponents = "PrefabsByComponentsData";
    public static string PathInitStates = "InitStates";
    public static string PathItems = "Items";
    public static string PathBioms = "Bioms";
    public static string PathBCFGs = "BCFGs";
    public static string PathTypeGenerations = "TypeGenerations";
    public static string PathRecipes = "Recipes";
    public static string PathRecipeGroups = "Groups";
    public static float ChunkSize => TileSize * ChunkTilesSize;
}