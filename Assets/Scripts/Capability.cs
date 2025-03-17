using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Capability : MonoBehaviour
{
    public Tilemap tilemap;
    public Vector3Int cellPosition;

    [Header("人物属性")] public Attitude attitude; // 态度
    public float health;
    public float reactionSpeed; // 反应速度

    public float nextActionTime; // 下次行动的时间
    private GameTime gameTime;

    [Header("平滑移动")] public float smoothSpeed = 0.125f; // 平滑移动速度


    public void Start()
    {
        tilemap = GameObject.Find("Tilemap").GetComponent<Tilemap>();
        gameTime = GameObject.Find("GameTime").GetComponent<GameTime>();
        
        var worldPos = tilemap.GetCellCenterWorld(cellPosition);
        transform.position = worldPos; // 更新位置
        nextActionTime = reactionSpeed;
    }

    public void FixedUpdate()
    {
        // 转换为该单元格中心的世界坐标
        var worldPos = tilemap.GetCellCenterWorld(cellPosition);
        var smoothedPosition = Vector3.Lerp(transform.position, worldPos, smoothSpeed); // 平滑移动
        transform.position = smoothedPosition; // 更新位置
    }

    public void SetNextActionTime(float standardCostTime)
    {
        var costTime = standardCostTime / reactionSpeed;
        nextActionTime = gameTime.Now() + costTime;
    }

    // public void AIAct()
    // {
    //     switch (expression)
    //     {
    //         
    //     }
    // }
}

public enum Attitude
{
    Player, // 玩家
    Friendly, // 友好
    Neutral, // 中立
    Hostile // 敌对
}