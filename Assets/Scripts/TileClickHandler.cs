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
    private Tilemap tilemap;
    public GameObject tileInfoPanelPrefab; // UI预制体
    public Vector2 clickWorldPos; // 世界坐标
    public Vector3Int cellPos; // 单元格坐标


    private GameObject currentPanel; // 当前显示的UI实例
    private GameObject player;
    private MapGen mapGen;
    private GameTime gameTime;
    private bool isPathfinding = false;

    // UI组件引用
    private TMP_Text coordText;
    private TMP_Text tileTypeText;
    private Button closeButton;
    private Button pathfindingButton;
    private TMP_Text pathfindingButtonText;

    public void Start()
    {
        player = FindObjectOfType<MapGen>().player;
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
            Debug.Log($"点击了格子：{cellPos}\n瓦片类型：{tilemap.GetTile(cellPos)?.name ?? "null"} {mapGen.map.passMap[cellPos.x][cellPos.y]}");

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
        closeButton = currentPanel.transform.Find("CloseButton").GetComponent<Button>();
        pathfindingButton = currentPanel.transform.Find("PathfindingButton").GetComponent<Button>();
        pathfindingButtonText = pathfindingButton.GetComponentInChildren<TMP_Text>();

        if (isPathfinding)
        {
            pathfindingButtonText.text = "停止寻路";
        }

        // 设置UI内容
        coordText.text = $"坐标：{cellPos}";
        tileTypeText.text = $"瓦片类型：{tilemap.GetTile(cellPos)?.name ?? "null"} {mapGen.map.passMap[cellPos.x][cellPos.y]}";

        // 设置按钮事件
        closeButton.onClick.AddListener(() => Destroy(currentPanel));
        pathfindingButton.onClick.AddListener(() => Goto());
    }


    public void Goto()
    {
        if (isPathfinding)
        {
            isPathfinding = false;
            return;
        }
        player.GetComponent<Capability>().SetNextActionTime(1f); // 设置下一次行动时间，也就是本次行动结束时间
        isPathfinding = true;
        pathfindingButtonText.text = "停止寻路";
        var capability = player.GetComponent<Capability>();
        Tuple<int, int> start = new(capability.cellPosition.x, capability.cellPosition.y);
        Tuple<int, int> end = new(cellPos.x, cellPos.y);
        Debug.Log($"正在寻路：从 {start} 到 {end}");
        var path = AStarPathfinding.AStar(mapGen.map.passMap, start, end);
        if (path.Count == 0)
        {
            Debug.Log("无法到达");
            return;
        }
        StartCoroutine(MoveAlongPath(path, capability)); // 启动协程
        Debug.Log("寻路结束");
        
    }
    
    public IEnumerator MoveAlongPath(List<Tuple<int, int>> path, Capability capability)
    {
        foreach (var p in path)
        {
            if (isPathfinding)
            {
                Debug.Log($"移动到：{p}");
                mapGen.map.passMap[p.Item1][p.Item2] = 1;
                mapGen.map.passMap[capability.cellPosition.x][capability.cellPosition.y] = 0;
                // capability.cellPosition = new Vector3Int(p.Item1, p.Item2, 0);
                var vectorDirection =
                    Actions.VectorDirection(capability.cellPosition, new Vector3Int(p.Item1, p.Item2, 0));
                Actions.Move(player, mapGen.map, vectorDirection);

                yield return new WaitForSeconds(0.5f); // 等待

                gameTime.NPCAct();
            }
            else
            {
                Debug.Log("寻路中止");
            }
            
        }
        isPathfinding = false;
        pathfindingButtonText.text = "前往";
    }
}