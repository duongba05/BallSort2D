﻿using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameGraphic : MonoBehaviour
{
    public int selectedBottleIndex = -1;
    public Game game;
    public List<BottleGraphic> bottleGraphics;// ống 
    public BallGraphic prefabBallGraphic; // tạo bóng mới
    public BottleGraphic prefabBottleGraphic; // tạo ống mới 
    private BallGraphic previewBall; // xem trước 
    public Vector3 bottleStartPosition; // vị trí ban đầu ống 
    public Vector3 bottleDistance; // khoảng cách 2 ống
    private void Start()
    {
        game = FindObjectOfType<Game>();
        selectedBottleIndex = -1;

        previewBall =Instantiate(prefabBallGraphic);
    }
    public void CreateBottleGraphic(List<Game.Bottle> bottles) // tạo giao diện các ống nghiệm 
    {
        foreach (Game.Bottle b in bottles)
        {
            BottleGraphic bg = Instantiate(prefabBottleGraphic);

            bottleGraphics.Add(bg);

            List<int> ballTypes = new List<int>();

            foreach (var ball in b.balls)
            {
                ballTypes.Add(ball.type);
            }
            bg.SetGraphic(ballTypes.ToArray());
        }
        Vector3 pos = bottleStartPosition;
        for (int i = 0; i < bottleGraphics.Count; i++)
        {
            bottleGraphics[i].transform.position = pos;
            pos.x += bottleDistance.x;
            bottleGraphics[i].index = i;
        }
    }
    public void RefreshBottleGraphic(List<Game.Bottle> bottles) // cập nhật giao diện các ống 
    {
        for(int i = 0; i < bottles.Count; i++)// duyệt từng ống 
        {
            Game.Bottle gb = bottles[i];
            BottleGraphic bottleGraphic = bottleGraphics[i];

            List<int> ballTypes = new List<int>();

            foreach(var ball in gb.balls)
            {
                ballTypes.Add(ball.type);   
            }
            bottleGraphic.SetGraphic(ballTypes.ToArray());
        }
    }
    public void OnClickBottle(int bottleIndex) // xử lý người chơi nhấn vào ống 
    {
        Debug.Log("Click bottle index: " + bottleIndex);

        if (isSwitchingBall) return;

        if (game.bottles[bottleIndex].balls.Count == 0 || game.IsBottleComplete(bottleIndex))
        {
            return;
        }
        if (selectedBottleIndex == -1)
        {
            if (game.bottles[bottleIndex].balls.Count != 0)
            {
                selectedBottleIndex = bottleIndex;
                StartCoroutine(MoveBallUp(bottleIndex)); // di chuyển bóng lên 
            }
        }
        else
        {
            if (selectedBottleIndex == bottleIndex)
            {
                StartCoroutine(MoveBallDown(bottleIndex)); // di chuyển bóng xuống 
                selectedBottleIndex = -1;
            }
            else
            {
                StartCoroutine(SwitchBallCoroutine(selectedBottleIndex, bottleIndex)); // nhấn ống khác thì chuyển bóng 
            }
        }
    }

    private IEnumerator MoveBallUp(int bottleIndex)
    {
        if (game.IsBottleComplete(bottleIndex))
        {
            Debug.Log("Bottle is complete, skipping MoveBallUp.");
            yield break;
        }

        isSwitchingBall = true;
        List<Game.Ball> ballList = game.bottles[bottleIndex].balls;
        Vector3 upPosition = bottleGraphics[bottleIndex].GetBottleUpPosition();
        Game.Ball b = ballList[ballList.Count - 1];
        Vector3 ballPosition = bottleGraphics[bottleIndex].GetBallPosition(ballList.Count - 1);
        bottleGraphics[bottleIndex].SetGraphicNone(ballList.Count - 1);
        previewBall.SetColor(b.type);
        previewBall.transform.position = ballPosition;
        previewBall.gameObject.SetActive(true);

        while (Vector3.Distance(previewBall.transform.position, upPosition) > 0.005f)
        {
            previewBall.transform.position = Vector3.MoveTowards(previewBall.transform.position, upPosition, 10 * Time.deltaTime);
            yield return null;
        }
        isSwitchingBall = false;
    }

    private IEnumerator MoveBallDown(int bottleIndex)
    {
        isSwitchingBall = true;
        List<Game.Ball> ballList = game.bottles[bottleIndex].balls;

        Vector3 downPosition = bottleGraphics[bottleIndex].GetBallPosition(ballList.Count - 1);
        Vector3 ballPosition = bottleGraphics[bottleIndex].GetBottleUpPosition();
        previewBall.transform.position = ballPosition;

        while (Vector3.Distance(previewBall.transform.position, downPosition) > 0.005f)
        {
            previewBall.transform.position = Vector3.MoveTowards(previewBall.transform.position, downPosition, 10 * Time.deltaTime);
            yield return null;
        }
        previewBall.gameObject.SetActive(false);

        Game.Ball b = ballList[ballList.Count - 1];
        bottleGraphics[bottleIndex].SetGraphic(ballList.Count - 1,b.type);
        isSwitchingBall = false;
    }
    private bool isSwitchingBall = false;
    private IEnumerator SwitchBallCoroutine(int frombottleIndex, int toBottleIndex) 
    {
        isSwitchingBall = true;

        List<Game.SwitchBallCommand> commands=game.CheckSwitchBall(frombottleIndex, toBottleIndex);

        if(commands.Count == 0)
        {
            Debug.Log("Cant move");
        }
        else
        {
            pendingBalls = commands.Count;
            previewBall.gameObject.SetActive(false);
            for (int i = 0; i < commands.Count; i++) 
            {
                Game.SwitchBallCommand command = commands[i];
                Queue<Vector3> moveQueue = GetCommandPath(command);
                if (i == 0)
                {
                    moveQueue.Dequeue();
                }
                StartCoroutine(SwitchBall(command, moveQueue));
                yield return new WaitForSeconds(0.1f);
            }
            while (pendingBalls > 0)
            {
                yield return null;
            }
            Debug.Log("Moving complete");
            game.SwitchBall(frombottleIndex,toBottleIndex);
        }
        selectedBottleIndex= -1;
        isSwitchingBall = false;

    }
    private int pendingBalls =0;    

    private Queue<Vector3> GetCommandPath(Game.SwitchBallCommand command)
    {
        Queue<Vector3> queueMovement = new Queue<Vector3>();
        queueMovement.Enqueue(bottleGraphics[command.fromBottleIndex].GetBallPosition(command.fromBallIndex));
        queueMovement.Enqueue(bottleGraphics[command.fromBottleIndex].GetBottleUpPosition());
        queueMovement.Enqueue(bottleGraphics[command.toBottleIndex].GetBottleUpPosition());
        queueMovement.Enqueue(bottleGraphics[command.toBottleIndex].GetBallPosition(command.toBallIndex));

        return queueMovement;
    }
    private IEnumerator SwitchBall(Game.SwitchBallCommand command, Queue<Vector3> movement)
    {
        bottleGraphics[command.fromBottleIndex].SetGraphicNone(command.fromBallIndex);
        Vector3 spawnPosition = movement.Peek();
        var ballObject = Instantiate(prefabBallGraphic, spawnPosition ,Quaternion.identity);

        ballObject.SetColor(command.type);

        while (movement.Count > 0) 
        {
             Vector3 target = movement.Dequeue();  
            while(Vector3.Distance(ballObject.transform.position,target) > 0.005f)
            {
                ballObject.transform.position = Vector3.MoveTowards(ballObject.transform.position,target,10*Time.deltaTime);
                yield return null;
            }
        }   
        yield return null;
        Destroy(ballObject.gameObject);   
        bottleGraphics[command.toBottleIndex].SetGraphic(command.toBallIndex, command.type);
        pendingBalls--;

    }
}
 