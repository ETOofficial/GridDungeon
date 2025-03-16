using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform player; // 角色的Transform
    public float smoothSpeed = 0.125f; // 平滑移动速度
    public Vector3 offset = new(0, 0, -10); // 摄像头与角色之间的偏移量

    void LateUpdate()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; // 通过标签查找玩家
        if (player == null) return;
        Vector3 desiredPosition = player.position + offset; // 计算目标位置
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed); // 平滑移动
        transform.position = smoothedPosition; // 更新摄像头位置
    }
}