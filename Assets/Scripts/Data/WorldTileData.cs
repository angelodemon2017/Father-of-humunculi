using System;

public class WorldTileData
{
    public int Id;
    public string BiomKey;
    public int Xpos;
    public int Zpos;
    public int SeedMask => GameProcess.Instance.GameWorld.Seed.GetShaderMask() + Xpos + Zpos;

    public Action<int> ChangedId;

    public WorldTileData(int id, int xpos, int zpos, string biomKey)
    {
        Id = id;
        Xpos = xpos;
        Zpos = zpos;
        BiomKey = biomKey;
    }

    public void ChangePart(int id)
    {
        Id = id;
        ChangedId?.Invoke(Id);
    }
}