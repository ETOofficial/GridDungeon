using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Capability : MonoBehaviour
{
    // public Grid Grid;
    public Tilemap tilemap;
    public Vector3Int cellPosition;
    [Header("人物属性")] public String attitude; // 态度（中立、敌对、友好、玩家等）
    public float health;
    public float reactionSpeed; // 反应速度

    public float nextActionTime; // 下次行动的时间
    private GameTime gameTime;

    [Header("平滑移动")]
    public float smoothSpeed = 0.125f; // 平滑移动速度

    
    void Start()
    {
        tilemap = GameObject.Find("Tilemap").GetComponent<Tilemap>();
        gameTime = GameObject.Find("GameTime").GetComponent<GameTime>();
        Vector3 worldPos = tilemap.GetCellCenterWorld(cellPosition);
        transform.position = worldPos; // 更新位置
        nextActionTime = reactionSpeed;
        
    }
    void FixedUpdate()
    {
        // 转换为该单元格中心的世界坐标
        Vector3 worldPos = tilemap.GetCellCenterWorld(cellPosition);
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, worldPos, smoothSpeed); // 平滑移动
        transform.position = smoothedPosition; // 更新位置
    }
    void Update()
    {

    }

    public void SetNextActionTime(float standardCostTime)
    {
        float costTime = standardCostTime / reactionSpeed;
        nextActionTime = gameTime.Now() + costTime;
    }
}
