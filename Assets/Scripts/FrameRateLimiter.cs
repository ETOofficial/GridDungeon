using UnityEngine;

public class FrameRateLimiter : MonoBehaviour
{
    [Header("目标帧率")]
    public int targetFrameRate = 60;

    void Awake()
    {
        // 设置帧率上限
        Application.targetFrameRate = targetFrameRate;
    }
}