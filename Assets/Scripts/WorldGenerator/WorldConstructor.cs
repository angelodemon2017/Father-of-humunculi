﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WorldConstructor
{
    public static WorldTileData GenerateTile(int x, int z, SeedData seed, int typeGeneration = 0)
    {
        var typeGenerationSO = GenerationController.GetGeneration(typeGeneration);
        return typeGenerationSO.GenerateTile(x, z, seed);
    }

    public static List<EntityData> GenerateEntitiesByChunk(List<WorldTileData> chunk, SeedData seed, int typeGeneration = 0)
    {
        List<EntityData> result = new();

        //TODO!!!

        return result;
    }
}