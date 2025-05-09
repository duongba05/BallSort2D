﻿using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Game : MonoBehaviour
{
    public GameGraphic graphic;
    public List<Bottle> bottles;
    public Game game;

    public BottleGraphic bottleGraphic;
    public GameObject complete;

    public void LoadLevel(List<int[]> listArray)
    {
        if (graphic == null)
        {
            Debug.LogError("graphic chưa được gán! Hãy đảm bảo bạn đã kéo thả GameGraphic vào Inspector.");
            return;
        }

        bottles = new List<Bottle>();
        foreach (int[] arr in listArray)
        {
            Bottle b = new Bottle { balls = new List<Ball>() }; // Đảm bảo balls không bị null
            for (int i = 0; i < arr.Length; i++)
            {
                int element = arr[i];
                if (element == 0) continue;
                b.balls.Add(new Ball
                {
                    type = element
                });
            }
            bottles.Add(b);
        }

        graphic.CreateBottleGraphic(bottles);
    }
    public void PrintBottles()
    {
        Debug.Log("Bottles ========");
        StringBuilder sb= new StringBuilder();  
        for(int i =0; i< bottles.Count; i++)
        {
            Bottle b = bottles[i];
            sb.Append("Bottle " + (i+1) + ":");
            foreach(Ball ball in b.balls)
            {
                sb.Append(" " + ball.type);
                sb.Append(",");
            }
            Debug.Log(sb.ToString());
            sb.Clear();
        }
        bool isWin = CheckWinCondition();
        Debug.Log("Is Win: " + isWin);
    }
    public void SwitchBall(Bottle bottle1, Bottle bottle2)
    {
        List<Ball> bottle1Balls = bottle1.balls;
        List<Ball> bottle2Balls = bottle2.balls;

        if(bottle1Balls.Count == 0) return;
        if (bottle2Balls.Count == 4) return;


        int index = bottle1Balls.Count - 1; 
        Ball b = bottle1Balls[index];

        var type = b.type;

        if(bottle2Balls.Count> 0&& bottle2Balls[bottle2Balls.Count-1].type != type)
        {
            return;    
        }
        for (int i = index; i >=0; i--) 
        {
            Ball ball = bottle1Balls[i];
            if (ball.type == type)
            {
                bottle1Balls.RemoveAt(i);
                bottle2Balls.Add(ball);
                if(bottle2Balls.Count == 4)
                {
                    break;
                }
            }
            else
            {
                break;
            }
        }
    }
    public void SwitchBall(int bottleIndex1, int bottleIndex2) 
    {
        Bottle b1 = bottles[bottleIndex1];  
        Bottle b2 = bottles[bottleIndex2];
        SwitchBall(b1, b2);
        graphic.RefreshBottleGraphic(bottles);
    } 
    public List<SwitchBallCommand> CheckSwitchBall(int bottleIndex1, int bottleIndex2)
    {
        List<SwitchBallCommand> commands = new List<SwitchBallCommand>();
        Bottle bottle1 = bottles[bottleIndex1];
        Bottle bottle2 = bottles[bottleIndex2];

        List<Ball> bottle1Balls = bottle1.balls;
        List<Ball> bottle2Balls = bottle2.balls;

        if (bottle1Balls.Count == 0) return commands;
        if (bottle2Balls.Count == 4) return commands;

        int index = bottle1Balls.Count - 1;
        Ball b = bottle1Balls[index];

        var type = b.type;

        if (bottle2Balls.Count > 0 && bottle2Balls[bottle2Balls.Count - 1].type != type)
        {
            return commands;
        }

        int targetIndex = bottle2Balls.Count;

        for (int i = index; i >= 0; i--)
        {
            Ball ball = bottle1Balls[i];
            if (ball.type == type)
            {
                // i=> targetIndex / targetIndex++ / if targetIndex > =4 bbreak;
                int fromBallIndex = i;
                int toBallIndex = targetIndex;
                int fromBottleIndex = bottleIndex1;
                int toBottleIndex = bottleIndex2;

                commands.Add(new SwitchBallCommand
                {
                    type = type,    
                    fromBallIndex = fromBallIndex,
                    toBallIndex = toBallIndex,
                    fromBottleIndex = fromBottleIndex,
                    toBottleIndex = toBottleIndex,
                });
                targetIndex++;  
                //bottle1Balls.RemoveAt(i);
                //bottle2Balls.Add(ball);
                if (targetIndex == 4)
                {
                    break;
                }
            }
            else
            {
                break;
            }
        }

        return commands;
    }
    public class SwitchBallCommand
    {
        public int type;
        public int fromBottleIndex;
        public int fromBallIndex;

        public int toBottleIndex;
        public int toBallIndex;
    }
    public bool CheckWinCondition()
    {
        bool winFlag = true;

        foreach (Bottle bottle in bottles){
            if(bottle.balls.Count == 0)
                continue;
            if (bottle.balls.Count < 4)
            {
                winFlag =false;
                break;
            }
            bool sameTypeFlag = true;
            int type = bottle.balls[0].type;
            foreach(Ball ball in bottle.balls)
            {
                if(ball.type != type)
                {
                    sameTypeFlag = false;
                    break;
                }
            }
            if (!sameTypeFlag) 
            {
                winFlag=false;
                break;
            }

        }

        return winFlag; 
    }
    public class Bottle
    {
        public List<Ball> balls; 
    }
    public class Ball
    {
        public int type;
    }
    public bool IsBottleComplete(int bottleIndex)
    {
        Game.Bottle bottle = game.bottles[bottleIndex];

        if (bottle.balls.Count != 4) return false;

        int firstBallType = bottle.balls[0].type;
        foreach (var ball in bottle.balls)
        {
            if (ball.type != firstBallType) return false;
        }

        return true;
    }

}
