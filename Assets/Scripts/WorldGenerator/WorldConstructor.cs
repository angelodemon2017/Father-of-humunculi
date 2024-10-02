using System;
using System.Collections.Generic;

public class WorldConstructor
{
    public WorldTile[,] partGrid = new WorldTile[1,1];

    public void Generate(int width, int length, string seed, List<TextureEntity> textureEntities)
    {
        partGrid = new WorldTile[width, length];

        for (int x = 0; x < width; x++)
            for (int z = 0; z < length; z++)
            {
                var ms = textureEntities.GetRandom();
                partGrid[x, z] = new WorldTile(x,z, ms.Id);
            }
    }

    private void GenerateStartLocation()
    {

    }

    public static WorldChunkData GenerateChunk(int x, int y, string seed)
    {
        var chunk = new WorldChunkData(x, y);
        for (int xTile = 0; xTile < WorldChunkView.ChunkSize; xTile++)
            for (int zTile = 0; zTile < WorldChunkView.ChunkSize; zTile++)
            {
                
            }

        return chunk;
    }
}

public class WorldChunk
{
    public WorldTile[,] Tiles = new WorldTile[WorldChunkView.ChunkSize, WorldChunkView.ChunkSize];


}

public class WorldTile
{
    private WorldTileData TileData = new();

    public int Id => TileData.Id;
    public int Xpos => TileData.Xpos;
    public int Zpos => TileData.Zpos;

    public Action<int> ChangedId;

    public void ChangePart(int id)
    {
        TileData.Id = id;
        ChangedId?.Invoke(TileData.Id);
    }

    public WorldTile(WorldTileData tileData)
    {
        TileData = tileData;
    }

    public WorldTile(int x, int z, int id)
    {
        TileData.Id = id;
        TileData.Xpos = x;
        TileData.Zpos = z;
    }
}