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
        SpawnCharacter(playerPrefab, _gameTime, Attitude.Player, "Player");
        SpawnCharacter(_assetDatabaseLoader.LittleBJY, _gameTime, Attitude.Hostile, "bjy");
        _camera.GetComponent<CameraMove>().MoveTo(player.GetComponent<Capability>().cellPosition, tilemap);
    }
    
    public void SpawnCharacter(GameObject prefab, GameTime gameTime, Attitude attitude, string name)
    {
        var npc = Instantiate(prefab);
        var cap = npc.GetComponent<Capability>();
        gameTime.allCharacters.Add(npc);
        cap.name = name;
        cap.attitude = attitude;
        var spawnPos = map.RandomCellPos();
        map.passMap[spawnPos.x][spawnPos.y] = 1;
        cap.cellPosition = spawnPos;
    }
}