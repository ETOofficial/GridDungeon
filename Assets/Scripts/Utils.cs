using UnityEngine;

public static class Utils
{
    public static void Log(object msg)
    {
#if UNITY_EDITOR
        Debug.Log(msg);
#endif
    }
    
    
}

public enum Direction
{
    Up,
    Down,
    Left,
    Right
}

