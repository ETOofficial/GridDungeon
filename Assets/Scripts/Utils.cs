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
}