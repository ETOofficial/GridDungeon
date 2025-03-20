using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class GameTime : MonoBehaviour
{
    public bool pause;
    public List<GameObject> allCharacters = new();

    public float now; // 游戏刻
    

    public void Start()
    {
        now = 0f;
        pause = true;
    }

    public void Update()
    {
        if (pause) return;
    }
    
}