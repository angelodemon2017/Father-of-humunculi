using System;
using System.Collections.Generic;

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
}