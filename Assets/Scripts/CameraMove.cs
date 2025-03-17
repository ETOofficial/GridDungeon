using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform player; // 角色的Transform
    public float smoothSpeed = 0.125f; // 平滑移动速度
    public Vector3 offset = new(0, 0, -10); // 摄像头与角色之间的偏移量
    public float dragSpeed = 0.5f; // 滑动速度系数

    private Vector2 touchStartPos;
    private bool isDragging = false;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; // 通过标签查找玩家
        if (player == null) return;
        Vector3 desiredPosition = player.position + offset; // 计算目标位置
        transform.position = desiredPosition;
    }

    void Update()
    {
        HandleTouchInput();
    }

    public void MoveTo(Vector3 targetPos)
    {
        Vector3 desiredPosition = targetPos + offset; // 计算目标位置
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed); // 平滑移动
        transform.position = smoothedPosition; // 更新摄像头位置
    }

    private void HandleTouchInput()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    touchStartPos = touch.position;
                    isDragging = true;
                    break;

                case TouchPhase.Moved:
                    Vector2 delta = touch.position - touchStartPos;
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
        Vector3 newPosition = transform.position + movement;
        newPosition.z = offset.z; // 保持z轴不变
        transform.position = Vector3.Lerp(transform.position, newPosition, smoothSpeed);
    }
}