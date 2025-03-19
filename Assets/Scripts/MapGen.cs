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
    private AssetDatabaseLoader assetDatabaseLoader;
    private GameTime gameTime;

    public void Start()
    {
        // 引用
        assetDatabaseLoader = GameObject.Find("AssetDatabaseLoader").GetComponent<AssetDatabaseLoader>();
        _camera = GameObject.Find("Main Camera");
        playerPrefab = assetDatabaseLoader.Stickman;
        gameTime = GameObject.Find("GameTime").GetComponent<GameTime>();
        
        map = new Map(0, height, width, tilemap, assetDatabaseLoader);
        map.GenerateMap();
        map.DrawMap();
        map.GeneratePassMap();
        player = map.SpawnPlayer(playerPrefab, gameTime);
        map.SpawnNPC(assetDatabaseLoader.LittleBJY, gameTime);
        _camera.GetComponent<CameraMove>().MoveTo(player.GetComponent<Capability>().cellPosition, tilemap);
    }
}