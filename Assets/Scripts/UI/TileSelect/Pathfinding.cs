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
        Actions.Pathfinding(
            player:_player,
            gameTime:_gameTime,
            text:_text,
            cellPos:_cellPos,
            map:_mapGen.map
            );
    }
    
    
}
