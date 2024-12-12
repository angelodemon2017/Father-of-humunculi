using System;
using System.Collections.Generic;
using static OptimazeExtensions;

[Serializable]
public class ComponentSpawner : ComponentData
{
    public int NeedCounterForSpawn;
    public int MaxEntityNear;
    public float DistanceSpawnMin;
    public float DistanceSpawnMax;
    public string KeyEntity;
    public float DistanceDetach;
    public List<long> Entities = new();

    public ComponentSpawner() : base(TypeCache<ComponentSpawner>.IdType)
    {

    }
}