using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraMove : MonoBehaviour
{
    public GameObject player; // 角色的Transform
    public float smoothSpeed = 0.125f; // 平滑移动速度
    public Vector3 offset = new(0, 0, -10); // 摄像头与角色之间的偏移量
    public float dragSpeed = 0.5f; // 滑动速度系数

    private Vector2 touchStartPos;
    private bool isDragging = false;

    private void Start()
    {
        player = FindObjectOfType<MapGen>().player; // 通过标签查找玩家
        if (player == null) return;
        var desiredPosition = player.transform.position + offset; // 计算目标位置
        transform.position = desiredPosition;
    }

    public void Update()
    {
        HandleTouchInput();
        // transform.position.z = offset.z;
    }

    public void MoveTo(Vector3 targetPos)
    {
        var position = targetPos + offset; // 计算目标位置
        // var smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed); // 平滑移动
        transform.position = position; // 更新摄像头位置
        Utils.Print($"摄像机移动至 {position}");
    }
    public void MoveTo(Vector3Int targetPos, Tilemap tilemap)
    {
        var position = tilemap.GetCellCenterWorld(targetPos);
        MoveTo(position);
    }

    private void HandleTouchInput()
    {
        if (Input.touchCount == 1)
        {
            var touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    touchStartPos = touch.position;
                    isDragging = true;
                    break;

                case TouchPhase.Moved:
                    var delta = touch.position - touchStartPos;
                    MoveCamera(dragSpeed * Time.deltaTime * new Vector3(-delta.x, -delta.y, 0));
                    touchStartPos = touch.position;
                    break;

                case TouchPhase.Ended:
                    isDragging = false;
                    break;
            }
        }
    }
    private void MoveCamera(Vector3 movement)
    {
        var newPosition = transform.position + movement;
        newPosition.z = offset.z; // 保持z轴不变
        transform.position = Vector3.Lerp(transform.position, newPosition, smoothSpeed);
    }
}