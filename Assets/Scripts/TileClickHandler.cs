using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System;
using System.Collections;

public class TileClickHandler : MonoBehaviour, IPointerClickHandler
{
    [Header("引用")]
    public Grid Grid;
    private Tilemap _tilemap;
    public GameObject tileInfoPanelPrefab; // UI预制体
    public Vector2 clickWorldPos; // 世界坐标
    public Vector3Int cellPos; // 单元格坐标


    public GameObject currentPanel; // 当前显示的UI实例
    private GameObject player;
    private MapGen mapGen;
    private GameTime _gameTime;
    private bool isPathfinding = false;

    // UI组件引用
    private TMP_Text coordText;
    private TMP_Text tileTypeText;
    private Button closeButton;
    private Button pathfindingButton;
    private TMP_Text pathfindingButtonText;

    public void Start()
    {
        _gameTime = FindObjectOfType<GameTime>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _tilemap = Grid.transform.Find("Tilemap").GetComponent<Tilemap>();
        // 获取MapGen组件引用
        mapGen = FindObjectOfType<MapGen>();

        // 如果已经存在UI面板，先关闭
        if (currentPanel != null)
        {
            Destroy(currentPanel);
        }

        // 将点击位置转换为世界坐标
        clickWorldPos = Camera.main.ScreenToWorldPoint(eventData.position);

        // 获取点击的单元格坐标
        cellPos = _tilemap.WorldToCell(clickWorldPos);

        // 检查该位置是否有瓦片
        if (_tilemap.HasTile(cellPos))
        {
            Utils.Print($"点击了格子：{cellPos}\n瓦片类型：{_tilemap.GetTile(cellPos)?.name ?? "null"} {mapGen.map.passMap[cellPos.x][cellPos.y]}");

            // 调用其他逻辑（例如触发宝箱、战斗等）
            ShowTileSelectUI(cellPos);
        }
    }

    private void ShowTileSelectUI(Vector3Int cellPos)
    {
        // 实例化UI预制体
        currentPanel = Instantiate(tileInfoPanelPrefab);

        // 将UI设置为Canvas的子对象
        currentPanel.transform.SetParent(FindObjectOfType<Canvas>().transform, false);

        // 获取UI组件引用
        coordText = currentPanel.transform.Find("CoordText").GetComponent<TMP_Text>();
        tileTypeText = currentPanel.transform.Find("TileTypeText").GetComponent<TMP_Text>();

        if (isPathfinding)
        {
            pathfindingButtonText.text = "停止寻路";
        }

        // 设置UI内容
        coordText.text = $"坐标：{cellPos}";
        tileTypeText.text = $"瓦片类型：{_tilemap.GetTile(cellPos)?.name ?? "null"} {mapGen.map.passMap[cellPos.x][cellPos.y]}";
    }
}