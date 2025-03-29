using UnityEngine;

public static class Utils
{
    /// <summary>
    /// 打印信息（仅用于调试，正式版中无任何效果）
    /// </summary>
    /// <param name="msg">要打印的信息</param>
    public static void Print(object msg)
    {
#if UNITY_EDITOR
        Debug.Log(msg);
#endif
    }

    public static int TileLen(Vector3Int start, Vector3Int end)
    {
        return Mathf.Abs(start.x - end.x) + Mathf.Abs(start.y - end.y);
    }
}