using System.Collections.Generic;

public class WorldData
{
    public string Name;
    public string Seed;

    public List<WorldTileData> worldPartDatas = new();

    public List<EntityData> entityDatas = new();
}

public class WorldTileData
{
    public int Id;
    public int Xpos;
    public int Zpos;
}

public class EntityData
{
    public List<ComponentData> Components;
}

public abstract class ComponentData
{

}

public class ComponentHPData : ComponentData
{
    public int CurrentHP;
    public int MaxHP;
    public int RegenHP;//persecond
}

public class ComponentContainerData
{
    public string ComponentName;
    public string ContainerContent;
}