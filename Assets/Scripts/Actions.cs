using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    /// <summary>
    /// 随机移动
    /// </summary>
    /// <param name="obj">要移动的对象</param>
    /// <param name="map">地图</param>
    public static void RandomMove(GameObject obj, Map map)
    {
        var randomDirection = RandomDirection();
        var capability = obj.GetComponent<Capability>();
        if (map.passMap[capability.cellPosition.x + randomDirection.x][capability.cellPosition.y + randomDirection.y] ==
            0) Move(obj, map, randomDirection);
    }

    /// <summary>
    /// 随机方向向量
    /// </summary>
    /// <returns>方向向量</returns>
    public static Vector3Int RandomDirection()
    {
        var randint = Random.Range(0, 4);
        return randint switch
        {
            0 => Vector3Int.up,
            1 => Vector3Int.down,
            2 => Vector3Int.left,
            3 => Vector3Int.right,
        };
    }

    public static void Atk(GameObject target, GameObject atker)
    {
    }

    public static void Pathfinding(GameObject player, GameTime gameTime, TMP_Text text, Vector3Int cellPos, Map map)
    {
        var cap = player.GetComponent<Capability>();
        if (cap.isPathfinding)
        {
            cap.isPathfinding = false;
            return;
        }

        if (cap == null) Debug.LogError("找不到玩家");
        cap.SetNextActionTime(1f, gameTime); // 设置下一次行动时间
        cap.isPathfinding = true;
        if (cap.attitude == Attitude.Player) text.text = "停止寻路"; // 设置按钮文本
        Tuple<int, int> start = new(cap.cellPosition.x, cap.cellPosition.y);
        Tuple<int, int> end = new(cellPos.x, cellPos.y);
        Utils.Print($"正在寻路：从 {start} 到 {end}");
        var path = AStarPathfinding.AStar(map.passMap, start, end);
        if (path.Count == 0)
        {
            Utils.Print("无法到达");
            return;
        }

        cap.StartCoroutine(MoveAlongPath(path, cap, map, gameTime, player, text)); // 启动协程
        Utils.Print("寻路结束");
    }

    private static IEnumerator MoveAlongPath(List<Tuple<int, int>> path, Capability cap, Map map,
        GameTime gameTime, GameObject player, TMP_Text text)
    {
        foreach (var p in path)
        {
            if (cap.isPathfinding)
            {
                Utils.Print($"移动到：{p}");
                map.passMap[p.Item1][p.Item2] = 1;
                map.passMap[cap.cellPosition.x][cap.cellPosition.y] = 0;


                // 先执行先于玩家的NPC行动
                AI.NPCAct(gameTime, map);

                // 执行玩家行动
                cap.SetNextActionTime(1f, gameTime);
                Utils.Print($"{cap.name} 开始行动\n开始时间： {gameTime.Get()}\n结束时间：{cap.nextActionTime}");
                var vectorDirection =
                    VectorDirection(cap.cellPosition, new Vector3Int(p.Item1, p.Item2, 0));
                var moveSuccess = Move(player, map, vectorDirection);
                if (!moveSuccess)
                {
                    cap.isPathfinding = false;
                }

                gameTime.Set(cap.nextActionTime); // 更新时间
                Utils.Print($"{cap.name} 结束行动\n结束时间：{cap.nextActionTime}\n当前时间：{gameTime.Get()}");

                yield return new WaitForSeconds(0.5f); // 等待
            }
            else
            {
                Utils.Print("寻路中止");
            }
        }

        cap.isPathfinding = false;
        if (cap.attitude == Attitude.Player) text.text = "前往";
    }
}