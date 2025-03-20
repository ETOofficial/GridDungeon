using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = System.Random;

public class AI
{
    public static void NPCAct(List<GameObject> ActionCharacters, GameTime gameTime, Map map)
    {
        Utils.Print("开始执行NPC行动");
        while (true)
        {
            var nextActor = EarliestActCharacter(ActionCharacters, gameTime);
            var capability = nextActor.GetComponent<Capability>();
            if (capability.attitude == Attitude.Player) break;
            // 执行NPC行动
            capability.SetNextActionTime(1f, gameTime);
            Utils.Print($"{capability.name} 开始行动\n开始时间： {gameTime.now}");
            Actions.RandomMove(nextActor, map);
            gameTime.now = capability.nextActionTime; // 更新时间
            Utils.Print($"{capability.name} 结束行动\n结束时间： {gameTime.now}\n当前时间：{gameTime.now}");
        }
        Utils.Print("NPC行动结束");
    }

    public static void NPCAct(GameTime gameTime, Map map)
    {
        var actionCharacters = gameTime.allCharacters;
        NPCAct(actionCharacters, gameTime, map);
    }

    public static GameObject EarliestActCharacter(List<GameObject> ActionCharacters, GameTime gameTime)
    {
        if (ActionCharacters.Count == 0) return null;
        var earliestActTime = 0f;
        GameObject nextActor = null;
        foreach (var character in ActionCharacters)
        {
            var capability = character.GetComponent<Capability>();
            if (capability.nextActionTime < earliestActTime || earliestActTime == 0f)
            {
                earliestActTime = capability.nextActionTime;
                nextActor = character;
            }
        }

        if (earliestActTime < gameTime.now)
        {
            Debug.LogError($"行动时间错误\n错误对象名称：{nextActor.name}\n错误原因：行动时间早于当前时间");
            // throw new Exception($"行动时间错误\n错误对象名称：{nextActor.name}\n错误原因：行动时间早于当前时间");
        }
        return nextActor;
    }
}