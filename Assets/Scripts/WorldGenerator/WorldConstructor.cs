using System;
using System.Collections.Generic;

public class WorldConstructor
{
    public WorldTile[,] partGrid = new WorldTile[1,1];

/*    public void Generate(int width, int length, string seed, List<TextureEntity> textureEntities)
    {
        partGrid = new WorldTile[width, length];

        for (int x = 0; x < width; x++)
            for (int z = 0; z < length; z++)
            {
                var ms = textureEntities.GetRandom();
                partGrid[x, z] = new WorldTile(x,z, ms.Id);
            }
    }

    public static WorldChunk GenerateChunk(int x, int z, string seed, List<TextureEntity> textureEntities)
    {
        var chunk = new WorldChunk(x, z);
        for (int xTile = 0; xTile < Config.ChunkTilesSize; xTile++)
            for (int zTile = 0; zTile < Config.ChunkTilesSize; zTile++)
            {
                var ms = textureEntities.GetRandom();
                chunk.Tiles[xTile,zTile] =
                    new WorldTile(x * Config.ChunkTilesSize + xTile,
                    z * Config.ChunkTilesSize + zTile, ms.Id);
            }

        return chunk;
    }/**/

    public static WorldTileData GenerateTile(int x, int z)
    {
        var txt = WorldViewer.Instance.Textures.GetRandom();
        return new WorldTileData(txt.Id, x, z);
    }
}
/*??
public class WorldChunk
{
    public int Xpos;
    public int Zpos;

    public WorldTile[,] Tiles = new WorldTile[Config.ChunkTilesSize, Config.ChunkTilesSize];

    public WorldChunk(int x, int z)
    {
        Xpos = x;
        Zpos = z;
    }
}/**/

public class WorldTile
{
    private WorldTileData TileData;

    public int Id => TileData.Id;
    public int Xpos => TileData.Xpos;
    public int Zpos => TileData.Zpos;
    public int SwiftSeed => TileData.SeedMask;

    public Action<int> ChangedId;

    public void SetNewId(int id)
    {
        TileData.ChangePart(id);
    }

    public void ChangedPart(int id)
    {
        ChangedId?.Invoke(TileData.Id);
    }

    public WorldTile(WorldTileData tileData)
    {
        TileData = tileData;
        TileData.ChangedId += ChangedPart;
    }

    public WorldTile(int x, int z, int id)
    {
        TileData = new WorldTileData(id, x, z);
    }
}