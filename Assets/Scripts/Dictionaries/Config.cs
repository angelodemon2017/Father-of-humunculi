public static class Config
{
    public static int ChunkTilesSize = 3;
    public static float TileSize = 2f;
    public static int VisibilityChunkDistance = 3;

    public static float ChunkSize => TileSize * ChunkTilesSize;
}