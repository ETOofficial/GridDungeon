using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Tilemaps;

public class MapGen : MonoBehaviour
{
    private GameObject playerPrefab;

    [Header("地图参数")] public int width; // 地图宽度
    public int height; // 地图高度

    [Header("引用")] public List<List<NBTTile>> map; // 生成的地图
    public Tilemap tilemap; // 绑定的Tilemap组件

    [Header("瓦片")] 


    public int[][] passMap;
    private GameObject player;

    private GameObject __camera;
    private AssetDatabaseLoader assetDatabaseLoader;
    
    private 

    void Start()
    {
        assetDatabaseLoader = GameObject.Find("AssetDatabaseLoader").GetComponent<AssetDatabaseLoader>();

        __camera = GameObject.Find("Main Camera");
        playerPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Characters/Stickman.prefab");
        GenerateMap();
        SpawnPlayer();
        SpawnNPC();
        passMap = GetPassMap(map);
        // TODO passMap逻辑需要更新
    }

    public void GenerateMap()
    {
        tilemap.ClearAllTiles(); // 清空旧地图

        map ??= new List<List<NBTTile>>();
        // 初始化列表
        Debug.Log("正在初始化地图……");
        for (int x = 0; x < width; x++)
        {
            List<NBTTile> row = new();
            for (int y = 0; y < height; y++)
            {
                row.Add(new NBTTile());
            }

            map.Add(row);
        }

        // 生成地图
        Debug.Log("正在生成地图……");
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // 生成边界墙
                if (x == 0 || y == 0 || x == width - 1 || y == height - 1)
                {
                    map[x][y] = new NBTTile()
                    {
                        tile = assetDatabaseLoader.wallTile,
                        nbt = new Dictionary<string, object>()
                        {
                            { "passable", false },
                            { "modable", false }
                        }
                    };
                }
                else if (Random.Range(0, 100) < 10)
                {
                    map[x][y] = new NBTTile()
                    {
                        tile = assetDatabaseLoader.woodenBoxTile,
                        nbt = new Dictionary<string, object>()
                        {
                            { "passable", false },
                        }
                    };
                }
                else
                {
                    map[x][y] = new NBTTile() { tile = assetDatabaseLoader.groundTile };
                }
            }
        }

        // 绘制地图
        Debug.Log("正在绘制地图……");
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3Int tilePos = new(x, y, 0); // 居中坐标
                tilemap.SetTile(tilePos, map[x][y].tile);
            }
        }

        Debug.Log("地图生成完毕！");
    }

    public void SpawnPlayer()
    {
        player = Instantiate(playerPrefab);
        player.tag = "Player"; // 添加标签
        Vector3Int spawnPos = new(2, 2);
        player.GetComponent<Capability>().cellPosition = spawnPos;
        __camera.transform.position = spawnPos + __camera.GetComponent<CameraMove>().offset;
    }

    public void SpawnNPC()
    {
        // GameObject npc = Instantiate();
    }

    public int[][] GetPassMap(List<List<NBTTile>> map)
    {
        int[][] passMap = new int[map.Count][];
        for (int i = 0; i < map.Count; i++)
        {
            passMap[i] = new int[map[i].Count];
            for (int j = 0; j < map[i].Count; j++)
            {
                if ((bool)map[i][j].TryGetNBT("passable", true) && (bool)map[i][j].TryGetNBT("has_effect", true))
                {
                    passMap[i][j] = 0;
                }
                else
                {
                    passMap[i][j] = 1;
                }
            }
        }

        return passMap;
    }
}