using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class GameTime : MonoBehaviour
{
    public bool pause;
    public List<GameObject> allCharacters = new();

    public float now = 0f; // 游戏刻
    

    void Start()
    {
        pause = true;
    }

    void Update()
    {
        if (pause) return;
    }

    /**
     * 行动基本设定：
     * 1.指定要做的动作
     * 2.计算所需时间
     * 3.找到最先完成行动的目标，执行动作，并将当前游戏刻设定为完成时间，并继续循环
     */
    public void NPCAct()
    {
        GameObject nextActor = null;
        var earliest = 0f;
        // 重复直至玩家行动
        while (true)
        {
            // 遍历处理每个对象
            foreach (var obj in allCharacters)
            {
                var nextActionTime = obj.GetComponent<Capability>().nextActionTime;
                // 如果该对象下一次行动的时间早于当前最早行动时间，则更新最早行动时间和下一个对象
                if (earliest == 0f || nextActionTime < earliest)
                {
                    earliest = nextActionTime;
                    nextActor = obj;
                }
            }

            if (nextActor.GetComponent<Capability>().attitude == Attitude.Player)
            {
                break;
            }
            // TODO 执行动作
            now = earliest;
        }
        Utils.Print($"当前刻度： {now}");
    }
}