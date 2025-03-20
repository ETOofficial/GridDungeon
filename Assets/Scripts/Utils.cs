using UnityEngine;

public static class Utils
{
    /**
     * 仅用于调试，正式版中不会有任何效果
     */
    public static void Print(object msg)
    {
#if UNITY_EDITOR
        Debug.Log(msg);
#endif
    }
}