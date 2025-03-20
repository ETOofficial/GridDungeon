using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGen : MonoBehaviour
{
    private GameObject playerPrefab;
    public GameObject player;

    [Header("地图参数")] public int width; // 地图宽度
    public int height; // 地图高度
    public Map map; // 生成的地图
    public Tilemap tilemap; // 绑定的Tilemap组件
    

    private GameObject _camera;
    private AssetDatabaseLoader _assetDatabaseLoader;
    private GameTime _gameTime;

    public void Start()
    {
        // 引用
        _assetDatabaseLoader = GameObject.Find("AssetDatabaseLoader").GetComponent<AssetDatabaseLoader>();
        _camera = GameObject.Find("Main Camera");
        playerPrefab = _assetDatabaseLoader.Stickman;
        _gameTime = GameObject.Find("GameTime").GetComponent<GameTime>();
        
        map = new Map(0, height, width, tilemap, _assetDatabaseLoader);
        map.GenerateMap();
        map.GeneratePassMap();
        map.DrawMap();
        SpawnPlayer(playerPrefab, _gameTime);
        SpawnNPC(_assetDatabaseLoader.LittleBJY, _gameTime);
        _camera.GetComponent<CameraMove>().MoveTo(player.GetComponent<Capability>().cellPosition, tilemap);
    }
    
    public void SpawnPlayer(GameObject playerPrefab, GameTime gameTime)
    {
        player = Instantiate(playerPrefab);
        var capability = player.GetComponent<Capability>();
        gameTime.allCharacters.Add(player); // 将player添加到游戏时间管理器中
        capability.attitude = Attitude.Player;
        capability.name = "Player";
        var spawnPos = map.RandomCellPos();
        capability.cellPosition = spawnPos;
        map.passMap[spawnPos.x][spawnPos.y] = 1;
        // __camera.GetComponent<CameraMove>().MoveTo(tilemap.GetCellCenterWorld(spawnPos));
    }
    
    
    public void SpawnNPC(GameObject NPCPrefab, GameTime gameTime)
    {
        var npc = Instantiate(NPCPrefab);
        // if (npc == null) Debug.LogError("NPC生成失败");
        var capability = npc.GetComponent<Capability>();
        gameTime.allCharacters.Add(npc);
        capability.attitude = Attitude.Neutral;
        var spawnPos = map.RandomCellPos();
        map.passMap[spawnPos.x][spawnPos.y] = 1;
        capability.cellPosition = spawnPos;
        capability.SetNextActionTime(1f, gameTime);
    }
}