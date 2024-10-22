public static class Config
{
    public static int ChunkTilesSize = 3;
    public static float TileSize = 2f;
    public static int VisibilityChunkDistance = 4;
    public static int EntitiesInChunk = 1;

    public static string PathEntityModels = "EntityModels";
    public static string PathPrefabsByComponents = "PrefabsByComponentsData";
    public static string PathInitStates = "InitStates";
    public static string PathItems = "Items";

    public static float PerlinScale => WorldViewer.Instance.PERLINSCALE;
    public static float HolesMap => WorldViewer.Instance.HolesMap;
    public static float ChunkSize => TileSize * ChunkTilesSize;
}