using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Game : MonoBehaviour
{
    public GameGraphic graphic;
    public List<Bottle> bottles;
    private IEnumerator Start()
    {
        bottles = new List<Bottle>();
        bottles.Add(new Bottle
        {
            balls = new List<Ball> { new Ball {type = BallType.RED }, new Ball { type = BallType.RED } }
        });
        bottles.Add(new Bottle
        {
            balls = new List<Ball> { new Ball { type = BallType.RED }, new Ball { type = BallType.RED } }
        });
        graphic.RefreshBottleGraphic(bottles);
        yield return new WaitForSeconds(2f); 
        //PrintBottles();
        //switch ball tu 1 sang 2
        SwitchBall(bottles[0], bottles[1]);
        graphic.RefreshBottleGraphic(bottles);
        //PrintBottles();
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
        if (bottle1Balls.Count == 4) return;


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
            }
            else
            {
                break;
            }
        }
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
            BallType type = bottle.balls[0].type;
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
        public BallType type;
    }
    public enum BallType
    {
        RED,
        GREEN,
        BLUE,
        ORANGE,
        MAGNETA,
        CYAN,
        BROWN
    }
}
