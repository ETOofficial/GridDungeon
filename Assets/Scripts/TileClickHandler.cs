using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System;
using Unity.VisualScripting;

public class TileClickHandler : MonoBehaviour, IPointerClickHandler
{
    [Header("引用")]
    public Grid Grid;
    private Tilemap tilemap;
    public GameObject tileInfoPanelPrefab; // UI预制体
    public Vector2 clickWorldPos; // 世界坐标
    public Vector3Int cellPos; // 单元格坐标


    private GameObject currentPanel; // 当前显示的UI实例
    private Transform player; // 角色的Transform
    private MapGen mapGen;
    private GameTime gameTime;

    public void Start()
    {
        gameTime = FindObjectOfType<GameTime>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        tilemap = Grid.transform.Find("Tilemap").GetComponent<Tilemap>();
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
        cellPos = tilemap.WorldToCell(clickWorldPos);

        // 检查该位置是否有瓦片
        if (tilemap.HasTile(cellPos))
        {
            Debug.Log($"点击了格子：{cellPos}\n瓦片类型：{tilemap.GetTile(cellPos)?.name ?? "null"} {mapGen.passMap[cellPos.x][cellPos.y]}");

            // 调用其他逻辑（例如触发宝箱、战斗等）
            ShowTileSelectUI(cellPos, clickWorldPos);
        }
    }

    private void ShowTileSelectUI(Vector3Int cellPos, Vector2 clickWorldPos)
    {
        // 实例化UI预制体
        currentPanel = Instantiate(tileInfoPanelPrefab);

        // 将UI设置为Canvas的子对象
        currentPanel.transform.SetParent(FindObjectOfType<Canvas>().transform, false);

        // 获取UI组件引用
        TMP_Text coordText = currentPanel.transform.Find("CoordText").GetComponent<TMP_Text>();
        TMP_Text tileTypeText = currentPanel.transform.Find("TileTypeText").GetComponent<TMP_Text>();
        Button closeButton = currentPanel.transform.Find("CloseButton").GetComponent<Button>();
        Button pathfindingButton = currentPanel.transform.Find("PathfindingButton").GetComponent<Button>();

        // 设置UI内容
        coordText.text = $"坐标：{cellPos}";
        tileTypeText.text = $"瓦片类型：{tilemap.GetTile(cellPos)?.name ?? "null"} {mapGen.passMap[cellPos.x][cellPos.y]}";

        // 设置按钮事件
        closeButton.onClick.AddListener(() => Destroy(currentPanel));
        pathfindingButton.onClick.AddListener(() => Goto());

        // // 将UI定位到点击位置（可选）
        // PositionPanelAtWorldPos(clickWorldPos);
    }

    public void Goto()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; // 通过标签查找玩家
        Capability capability = FindObjectOfType<Capability>();
        Tuple<int, int> start = new(capability.cellPosition.x, capability.cellPosition.y);
        Tuple<int, int> end = new(cellPos.x, cellPos.y);
        List<Tuple<int, int>> path = AStarPathfinding.AStar(mapGen.passMap, start, end);
        if (path == new List<Tuple<int, int>>())
        {
            Debug.Log("无法到达");
            return;
        }
        foreach (var p in path)
        {
            gameTime.NPCAct(); // NPC 行动
            Debug.Log($"移动到：{p}");
            capability.cellPosition = new Vector3Int(p.Item1, p.Item2, 0);
        }
    }
        

    // 将UI定位到世界坐标（转换为屏幕坐标）
    private void PositionPanelAtWorldPos(Vector2 worldPos)
    {
        RectTransform panelRect = currentPanel.GetComponent<RectTransform>();
        Vector2 screenPos = Camera.main.WorldToScreenPoint(worldPos);
        panelRect.position = screenPos + new Vector2(20, 20); // 添加偏移防止遮挡
    }

    // // 添加触摸支持
    // void Update()
    // {
    //     if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
    //     {
    //         Vector2 touchPos = Input.GetTouch(0).position;
    //         OnPointerClick(new PointerEventData(EventSystem.current) { position = touchPos });
    //     }
    // }
}