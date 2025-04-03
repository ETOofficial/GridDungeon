using UnityEngine;
using UnityEngine.Tilemaps;

public class Capability : MonoBehaviour
{
    public Tilemap tilemap;
    public Vector3Int cellPosition;

    [Header("人物属性")] public new string name = "Unnamed";
    public Attitude attitude; // 态度
    public float health;
    public float reactionSpeed = 1f; // 反应速度
    public float nextActionTime; // 下次行动的时间
    public bool isPathfinding = false;
    // private GameTime _gameTime;

    [Header("平滑移动")] public float smoothSpeed = 0.125f; // 平滑移动速度


    public void Start()
    {
        tilemap = GameObject.Find("Tilemap").GetComponent<Tilemap>();
        // _gameTime = GameObject.Find("GameTime").GetComponent<GameTime>();
        
        var worldPos = tilemap.GetCellCenterWorld(cellPosition);
        transform.position = worldPos; // 更新位置
        nextActionTime = 1f; // 默认下次行动的时间
    }

    public void FixedUpdate()
    {
        // 转换为该单元格中心的世界坐标
        var worldPos = tilemap.GetCellCenterWorld(cellPosition);
        var smoothedPosition = Vector3.Lerp(transform.position, worldPos, smoothSpeed); // 平滑移动
        transform.position = smoothedPosition; // 更新位置
    }
    
    /// <summary>
    /// 设置下次行动的时间
    /// </summary>
    /// <param name="standardCostTime">标准消耗时间</param>
    /// <param name="gameTime">游戏时间</param>
    public void SetNextActionTime(float standardCostTime, GameTime gameTime)
    {
        if (reactionSpeed <= 0f) return; 
        var costTime = standardCostTime / reactionSpeed;
        nextActionTime = gameTime.Get() + costTime;
        Utils.Print($"{name} 下次行动的时间被设为 {nextActionTime}");
    }
}

public enum Attitude
{
    Player, // 玩家
    Friendly, // 友好
    Neutral, // 中立
    Hostile // 敌对
}