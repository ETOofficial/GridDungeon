using System;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

public class NBTTile
{
    public TileBase tile;
    public Dictionary<string, object> nbt;

    public void AddNBT(string key, object value)
    {
        nbt.Add(key, value);
    }

    public object GetNBT(string key)
    {
        return nbt[key];
    }

    public object TryGetNBT(string key, object defaultValue)
    {
        // 1. 检查字典是否存在且包含键
        if (nbt != null && nbt.ContainsKey(key) == true)
        {
            return nbt[key];
        }
        // 3. 失败时返回默认值
        return defaultValue;
    }

    public void SetNBT(string key, object value)
    {
        nbt[key] = value;
    }
}

