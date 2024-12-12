using System;

public class WorldTileData
{
    public int Id;
    public string BiomKey;
    public int Xpos;
    public int Zpos;
    public int SeedMask => GameProcess.Instance.GameWorld.Seed.GetShaderMask() + Xpos + Zpos;

    public Action<int> ChangedId;
    public string DebugText;

    public (int, int) GetChunkPos => (1,1);

    public WorldTileData(int id, int xpos, int zpos, string biomKey, string debugInfo = "")
    {
        Id = id;
        Xpos = xpos;
        Zpos = zpos;
        BiomKey = biomKey;
        DebugText = debugInfo;
    }

    public void ChangePart(int id)
    {
        Id = id;
        ChangedId?.Invoke(Id);
    }
}