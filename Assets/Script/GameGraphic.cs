using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class GameGraphic : MonoBehaviour
{
    public List<BottleGraphic> bottleGraphics;
     public void RefreshBottleGraphic(List<Game.Bottle> bottles)
    {
        for(int i = 0; i < bottles.Count; i++)
        {
            Game.Bottle gb = bottles[i];
            BottleGraphic bottleGraphic = bottleGraphics[i];

            List<Game.BallType> ballTypes = new List<Game.BallType>();

            foreach(var ball in gb.balls)
            {
                ballTypes.Add(ball.type);   
            }
            bottleGraphic.SetGraphic(ballTypes.ToArray());
        }
    }
}
