using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class Map
{
    public List<List<NBTTile>> map;
    public int layer; // 层数
    public int height;
    public int width;
    public int[][] passMap; // 通用passMap，理应不同实体不同
    
    private readonly AssetDatabaseLoader _assetDatabase; // 资源加载器
    private readonly Tilemap _tilemap;

    public Map(int layer, int height, int width, Tilemap tilemap, AssetDatabaseLoader loader)
    {
        this.layer = layer;
        this.height = height;
        this.width = width;
        map = new List<List<NBTTile>>();
        _tilemap = tilemap;
        _assetDatabase = loader;
    }

    public void GenerateMap()
    {
        // 初始化列表
        Utils.Print("正在初始化地图……");
        for (var x = 0; x < width; x++)
        {
            List<NBTTile> row = new();
            for (var y = 0; y < height; y++)
            {
                row.Add(new NBTTile());
            }

            map.Add(row);
        }

        // 生成地图
        Utils.Print("正在生成地图……");
        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y < height; y++)
            {
                // 生成边界墙
                if (x == 0 || y == 0 || x == width - 1 || y == height - 1)
                {
                    map[x][y] = new NBTTile()
                    {
                        tile = _assetDatabase.wallTile,
                        nbt = new Dictionary<string, object>()
                        {
                            { "passable", false },
                            { "modable", false }
                        }
                    };
                }
                else if (Random.Range(0, 100) < 10)
                {
                    map[x][y] = new NBTTile
                    {
                        tile = _assetDatabase.woodenBoxTile,
                        nbt = new Dictionary<string, object>()
                        {
                            { "passable", false },
                        }
                    };
                }
                else
                {
                    map[x][y] = new NBTTile { tile = _assetDatabase.groundTile };
                }
            }
        }
        Utils.Print("地图生成完毕！");
    }

    public void DrawMap()
    {
        // 绘制地图
        Utils.Print("正在绘制地图……");
        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y < height; y++)
            {
                Vector3Int tilePos = new(x, y, 0); // 居中坐标
                _tilemap.SetTile(tilePos, map[x][y].tile);
            }
        }
        Utils.Print("地图绘制完毕！");
    }
    
    public void GeneratePassMap()
    {
        passMap = new int[map.Count][];
        for (var i = 0; i < map.Count; i++)
        {
            passMap[i] = new int[map[i].Count];
            for (var j = 0; j < map[i].Count; j++)
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
    }
    
    public GameObject SpawnPlayer(GameObject playerPrefab, GameTime gameTime)
    {
        var player = GameObject.Instantiate(playerPrefab);
        gameTime.allCharacters.Add(player); // 将player添加到游戏时间管理器中
        player.GetComponent<Capability>().attitude = Attitude.Player;
        var spawnPos = RandomCellPos();
        player.GetComponent<Capability>().cellPosition = spawnPos;
        passMap[spawnPos.x][spawnPos.y] = 1;
        // __camera.GetComponent<CameraMove>().MoveTo(tilemap.GetCellCenterWorld(spawnPos));
        return player;
    }
    
    
    public void SpawnNPC(GameObject NPCPrefab, GameTime gameTime)
    {
        var npc = GameObject.Instantiate(NPCPrefab);
        gameTime.allCharacters.Add(npc);
        npc.GetComponent<Capability>().attitude = Attitude.Hostile;
        var spawnPos = RandomCellPos();
        passMap[spawnPos.x][spawnPos.y] = 1;
        npc.GetComponent<Capability>().cellPosition = spawnPos;
    }
    
    public Vector3Int RandomCellPos()
    {
        while (true)
        {
            Vector3Int pos = new(Random.Range(1, width), Random.Range(1, height));
            if (passMap[pos.x][pos.y] != 0) continue; 
            Utils.Print($"随机坐标：{pos}");
            return pos;
        }
    }
}

public enum Direction
{
    Up,
    Down,
    Left,
    Right
}