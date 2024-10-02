using System;
using System.Collections.Generic;

public class World
{
    public WorldPart[,] partGrid = new WorldPart[1,1];

    public void Generate(int width, int length, List<TextureEntity> textureEntities)
    {
        partGrid = new WorldPart[width, length];

        for (int x = 0; x < width; x++)
            for (int z = 0; z < length; z++)
            {
                var ms = textureEntities.GetRandom();
                partGrid[x, z] = new WorldPart(x,z, ms.Id);
            }
    }
}

public class WorldPart
{
    public int Id;
    public int Xpos;
    public int Zpos;

    public Action<int> ChangedId;

    public void ChangePart(int id)
    {
        Id = id;
        ChangedId?.Invoke(id);
    }

    public WorldPart(int x, int z, int id)
    {
        Id = id;
        Xpos = x;
        Zpos = z;
    }
}