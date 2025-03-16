using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;
public class GameTime : MonoBehaviour
{
    public bool pause;
    private float now = 0f;

    public float Now()
    {
        return now;
    }
    void Start()
    {
        pause = true;
    }

    void Update()
    {
        if (pause) return;
    }

    public void NPCAct()
    {
        List<GameObject> allTargets = new List<GameObject>();
        // 获取所有 Player 标签对象
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        allTargets.AddRange(players);
        // 获取所有 Friendly 标签对象
        try
        {
        GameObject[] friendlies = GameObject.FindGameObjectsWithTag("Friendly");
        allTargets.AddRange(friendlies);
        }
        catch (System.Exception)
        {
            
        }

        GameObject nextActor = null;
        float earliest = 0f;
        // 遍历处理每个对象
        foreach (GameObject obj in allTargets)
        {
            Capability capability = obj.GetComponent<Capability>();
            if (capability != null && capability.nextActionTime< now)
            {
                earliest = capability.nextActionTime;
                nextActor = obj;
            }
        }
        if (!players.Contains(nextActor))
        {
            
        }
        now = earliest;
    }
}
