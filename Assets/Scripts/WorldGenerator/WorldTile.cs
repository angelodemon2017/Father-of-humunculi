using System;

public class WorldTile
{
    private WorldTileData TileData;

    public int Id => TileData.Id;
    public int Xpos => TileData.Xpos;
    public int Zpos => TileData.Zpos;
    public int SwiftSeed => TileData.SeedMask;

    public Action<int> ChangedId;

    public string DebugData => TileData.DebugText;

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
}