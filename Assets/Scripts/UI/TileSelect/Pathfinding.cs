using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Pathfinding : MonoBehaviour
{
    private GameTime _gameTime;
    private TMP_Text _text;
    private MapGen _mapGen;
    private GameObject _player;
    private Capability _cap;
    private Vector3Int _cellPos;
    private void Start()
    {
        _gameTime = FindObjectOfType<GameTime>();
        _text = GetComponentInChildren<TMP_Text>();
        _mapGen = FindObjectOfType<MapGen>();
        _player = FindObjectOfType<MapGen>().player;
        _cap = _player.GetComponent<Capability>();
        _cellPos = FindObjectOfType<TileClickHandler>().cellPos;
        
        
        var btn = GetComponent<Button>();
        btn.onClick.AddListener(OnClick);
    }
    
    private void OnClick()
    {
        if (_cap.isPathfinding)
        {
            _cap.isPathfinding = false;
            return;
        }
        if (_cap == null) Debug.LogError("找不到玩家");
        _cap.SetNextActionTime(1f, _gameTime); // 设置下一次行动时间
        _cap.isPathfinding = true;
        _text.text = "停止寻路";
        Tuple<int, int> start = new(_cap.cellPosition.x, _cap.cellPosition.y);
        Tuple<int, int> end = new(_cellPos.x, _cellPos.y);
        Utils.Print($"正在寻路：从 {start} 到 {end}");
        var path = AStarPathfinding.AStar(_mapGen.map.passMap, start, end);
        if (path.Count == 0)
        {
            Utils.Print("无法到达");
            return;
        }
        StartCoroutine(MoveAlongPath(path, _cap)); // 启动协程
        Utils.Print("寻路结束");
    }
    
    private IEnumerator MoveAlongPath(List<Tuple<int, int>> path, Capability cap)
    {
        foreach (var p in path)
        {
            if (cap.isPathfinding)
            {
                Utils.Print($"移动到：{p}");
                _mapGen.map.passMap[p.Item1][p.Item2] = 1;
                _mapGen.map.passMap[cap.cellPosition.x][cap.cellPosition.y] = 0;
                
                // 先执行先于玩家的NPC行动
                AI.NPCAct(_gameTime, _mapGen.map);
                
                // 执行玩家行动
                cap.SetNextActionTime(1f, _gameTime);
                Utils.Print($"{cap.name} 开始行动\n开始时间： {_gameTime.now}\n结束时间：{cap.nextActionTime}");
                var vectorDirection =
                    Actions.VectorDirection(cap.cellPosition, new Vector3Int(p.Item1, p.Item2, 0));
                var moveSuccess = Actions.Move(_player, _mapGen.map, vectorDirection);
                if (!moveSuccess)
                {
                    cap.isPathfinding = false;
                }
                _gameTime.now = cap.nextActionTime; // 更新时间
                Utils.Print($"{cap.name} 结束行动\n结束时间：{cap.nextActionTime}\n当前时间：{_gameTime.now}");
                
                yield return new WaitForSeconds(0.5f); // 等待
            }
            else
            {
                Utils.Print("寻路中止");
            }
            
        }
        cap.isPathfinding = false;
        _text.text = "前往";
    }
}
