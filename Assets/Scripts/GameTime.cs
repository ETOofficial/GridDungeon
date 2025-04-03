using System.Collections.Generic;
using UnityEngine;


public class GameTime : MonoBehaviour
{
    public bool pause;
    public List<GameObject> allCharacters = new();

    private float now; // 游戏刻
    

    public void Start()
    {
        now = 0f;
        pause = true;
    }

    public void Update()
    {
        if (pause) return;
    }

    public void Set(float time)
    {
        now = time;
        Utils.Print($"游戏时间被设为 {now}");
    }

    public float Get()
    {
        return now;
    }
}