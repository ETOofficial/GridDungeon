using UnityEngine;

public static class Actions
{
    /// <param name="vectorDirection">方向向量，例如<see cref="Vector3Int.up"/></param>
    /// <returns>是否成功移动</returns>
    public static bool Move(GameObject obj, Map map, Vector3Int vectorDirection)
    {
        var cap = obj.GetComponent<Capability>();
        if (isOutOfMap(map, cap.cellPosition + vectorDirection))
        {
            return false;
        }
        obj.GetComponent<Capability>().cellPosition += vectorDirection;
        return true;
    }

    public static Vector3Int VectorDirection(Vector3Int cellPos, Vector3Int targetPos)
    {
        return targetPos - cellPos;
    }

    public static bool isOutOfMap(Map map, Vector3Int cellPos)
    {
        return cellPos.x < 0 || cellPos.x >= map.width || cellPos.y < 0 || cellPos.y >= map.height;
    }
}
