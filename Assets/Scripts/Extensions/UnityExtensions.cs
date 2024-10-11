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

    public static Vector3 GetChunkPos(this Vector3 globalPos)
    {

//        Vector3 delta
        return (Vector3)Vector3Int.RoundToInt(globalPos / Config.ChunkSize) * Config.ChunkSize;
    }
}