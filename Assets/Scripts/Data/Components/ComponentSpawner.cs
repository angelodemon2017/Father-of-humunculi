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
    public List<long> Entities = new();
}