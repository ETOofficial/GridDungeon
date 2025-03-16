using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Capability : MonoBehaviour
{
    // public Grid Grid;
    public Tilemap tilemap;
    public Vector3Int cellPosition;
    [Header("人物属性")]
    public float health;
    public float reactionSpeed; // 反应速度

    public float nextActionTime; // 下次行动的时间
    private GameObject gameTime;

    [Header("平滑移动")]
    public float smoothSpeed = 0.125f; // 平滑移动速度
    void Start()
    {
        gameTime = GameObject.Find("GameTime");
        Vector3 worldPos = tilemap.GetCellCenterWorld(cellPosition);
        transform.position = worldPos; // 更新位置
        nextActionTime = reactionSpeed;
    }
    void FixedUpdate()
    {
        tilemap = GameObject.Find("Tilemap").GetComponent<Tilemap>();
        // // 获取当前所在的单元格坐标
        // Vector3Int cellPosition = tilemap.WorldToCell(transform.position);
        // 转换为该单元格中心的世界坐标
        Vector3 worldPos = tilemap.GetCellCenterWorld(cellPosition);
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, worldPos, smoothSpeed); // 平滑移动
        transform.position = smoothedPosition; // 更新位置
    }
    void Update()
    {
        float now = gameTime.GetComponent<GameTime>().Now();
        if (now >= nextActionTime)
        {
            nextActionTime += reactionSpeed;
        }
    }
}
