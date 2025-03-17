using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;


public class AssetDatabaseLoader : MonoBehaviour
{
    [Header("人物")] public GameObject LittleBJY;
    [Header("瓦片")] public TileBase groundTile;
    public TileBase wallTile;
    public TileBase woodenBoxTile;

    void Start()
    {
        LittleBJY = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Characters/Little BJY.prefab");
        
        groundTile = AssetDatabase.LoadAssetAtPath<TileBase>("Assets/Textures/TileMap/Ground RT.asset"); // 空地瓦片
        wallTile = AssetDatabase.LoadAssetAtPath<TileBase>("Assets/Textures/TileMap/Wall RT.asset"); // 墙瓦片
        woodenBoxTile = AssetDatabase.LoadAssetAtPath<TileBase>("Assets/Textures/TileMap/Wooden Box RT.asset");
    }
}