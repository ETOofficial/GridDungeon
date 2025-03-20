using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public static class Actions
{
    /// <summary>
    /// 移动
    /// </summary>
    /// <param name="obj">要移动的对象</param>
    /// <param name="map">地图</param>
    /// <param name="vectorDirection">方向向量，例如<see cref="Vector3Int.up"/></param>
    /// <returns>是否成功移动</returns>
    public static bool Move(GameObject obj, Map map, Vector3Int vectorDirection)
    {
        var capability = obj.GetComponent<Capability>();
        var targetPos = capability.cellPosition + vectorDirection;
        // if (isOutOfMap(map, targetPos) || map.passMap[targetPos.x][targetPos.y] == 1) return false;
        if (isOutOfMap(map, targetPos)) return false;
        // TODO 可能触发机关等
        capability.cellPosition += vectorDirection;
        return true;
    }

    /// <summary>
    /// 方向向量
    /// </summary>
    /// <param name="startPos">起点</param>
    /// <param name="endPos">终点</param>
    /// <returns></returns>
    public static Vector3Int VectorDirection(Vector3Int startPos, Vector3Int endPos)
    {
        return endPos - startPos;
    }

    /// <summary>
    /// 判断是否超出地图
    /// </summary>
    /// <param name="map">地图</param>
    /// <param name="cellPos">位置</param>
    /// <returns>判断结果</returns>
    public static bool isOutOfMap(Map map, Vector3Int cellPos)
    {
        return cellPos.x < 0 || cellPos.x >= map.width || cellPos.y < 0 || cellPos.y >= map.height;
    }

    public static void RandomMove(GameObject obj, Map map)
    {
        var randomDirection = RandomDirection();
        Move(obj, map, randomDirection);
    }

    public static Vector3Int RandomDirection()
    {
        var randint = Random.Range(0, 4);
        return randint switch
        {
            0 => Vector3Int.up,
            1 => Vector3Int.down,
            2 => Vector3Int.left,
            3 => Vector3Int.right,
            _ => throw new IndexOutOfRangeException("随机方向出错")
        };
    }
}