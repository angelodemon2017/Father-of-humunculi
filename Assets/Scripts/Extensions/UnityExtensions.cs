using System.Collections.Generic;
using UnityEngine;

public static class UnityExtensions
{
    public static void DestroyChildrens(this Transform transform)
    {
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    public static Vector3Int GetChunkPosInt(this Vector3 globalPos)
    {
        return Vector3Int.RoundToInt(globalPos / Config.ChunkSize) * (int)Config.ChunkSize;
    }

    public static Vector3 GetChunkPos(this Vector3 globalPos)
    {
        return (Vector3)Vector3Int.RoundToInt(globalPos / Config.ChunkSize) * Config.ChunkSize;
    }

    public static List<T> GetNeigsElements<T>(this CashDictionary<Vector3Int, T> dict, Vector3 position) where T : IDictKey<Vector3Int>
    {
        List<T> result = new ();

        for (int x = -1; x <= 1; x++)
            for (int z = -1; z <= 1; z++)
            {
                var tempKey = position.GetChunkPosInt();
                tempKey.x += x;
                tempKey.z += z;
                if (dict.TryGetValue(tempKey, out List<T> cashes))
                {
                    cashes.RemoveAll(item => item == null);
                    result.AddRange(cashes);
                }
            }

        return result;
    }
}