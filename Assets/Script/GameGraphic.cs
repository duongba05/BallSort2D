using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameGraphic : MonoBehaviour
{
    public int selectedBottleIndex = -1;
    public Game game;
    public List<BottleGraphic> bottleGraphics;
    public BallGraphic prefabBallGraphic;
    private BallGraphic previewBall;
    private void Start()
    {
        game = FindObjectOfType<Game>();
        selectedBottleIndex = -1;

        previewBall =Instantiate(prefabBallGraphic);
    }
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
    public void OnClickBottle(int bottleIndex)
    {
        Debug.Log("Click bottle index: " + bottleIndex);
        if (isSwitchingBall) return;
        // trang thai mac dinh la tru 1 
        // trang thai co ball: bottleIndex 
        if (selectedBottleIndex == -1)
        {
            selectedBottleIndex = bottleIndex;
            StartCoroutine(MoveBallUp(bottleIndex));
        }
        else
        {
            if(selectedBottleIndex == bottleIndex)
            {
                StartCoroutine(MoveBallDown(bottleIndex));
                selectedBottleIndex=-1;
            }
            else
            {
                StartCoroutine(SwitchBallCoroutine(selectedBottleIndex, bottleIndex));
            }
            //game.SwitchBall(selectedBottleIndex, bottleIndex);
            //selectedBottleIndex = -1;
            //if (game.CheckWinCondition())
            //{
            //    Debug.Log("Win!");
            //}
        }
    }
    private IEnumerator MoveBallUp(int bottleIndex)
    {
        Vector3 upPosition = bottleGraphics[bottleIndex].GetBottleUpPosition();
        List<Game.Ball> ballList = game.bottles[bottleIndex].balls;
        // get highest ball
        Game.Ball b = ballList[ballList.Count - 1];
        Vector3 ballPosition = bottleGraphics[bottleIndex].GetBallPosition(ballList.Count - 1);
        bottleGraphics[bottleIndex].SetGraphicNone(ballList.Count - 1);
        previewBall.SetColor(BallGraphic.ConvertFromGameType(b.type));
        previewBall.transform.position = ballPosition;
        previewBall.gameObject.SetActive(true);
        
        while (Vector3.Distance(previewBall.transform.position, upPosition) > 0.005f)
        {
            previewBall.transform.position = Vector3.MoveTowards(previewBall.transform.position,upPosition,10*Time.deltaTime);    
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

        while (Vector3.Distance(previewBall.transform.position, downPosition) > 0.005f)
        {
            previewBall.transform.position = Vector3.MoveTowards(previewBall.transform.position, downPosition, 10 * Time.deltaTime);
            yield return null;
        }
        previewBall.gameObject.SetActive(false);

        Game.Ball b = ballList[ballList.Count - 1];
        bottleGraphics[bottleIndex].SetGraphic(ballList.Count - 1,b.type);
        //previewBall.transform.position = ballPosition;
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
            foreach (Game.SwitchBallCommand command in commands)
            {
                StartCoroutine(SwitchBall(command));
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
    private IEnumerator SwitchBall(Game.SwitchBallCommand command)
    {
        // tat graphic o vi tri from
        //tao 1 ball o vi tri from, co cung type
        // di chuyen ball theo dung duong 
        // xoa ball di chuyen bat graphic o vi tri to
        bottleGraphics[command.fromBottleIndex].SetGraphicNone(command.fromBallIndex);
        var ballObject = Instantiate(prefabBallGraphic, bottleGraphics[command.fromBottleIndex].GetBallPosition(command.fromBallIndex),Quaternion.identity);
        ballObject.SetColor(BallGraphic.ConvertFromGameType(command.type));
        Queue<Vector3> queueMovement = new Queue<Vector3>();
        queueMovement.Enqueue(bottleGraphics[command.fromBottleIndex].GetBallPosition(command.fromBallIndex));
        queueMovement.Enqueue(bottleGraphics[command.fromBottleIndex].GetBottleUpPosition());
        queueMovement.Enqueue(bottleGraphics[command.toBottleIndex].GetBottleUpPosition());
        queueMovement.Enqueue(bottleGraphics[command.toBottleIndex].GetBallPosition(command.toBallIndex));

        while (queueMovement.Count > 0) 
        {
             Vector3 target = queueMovement.Dequeue();  
            while(Vector3.Distance(ballObject.transform.position,target) > 0.005f)
            {
                ballObject.transform.position = Vector3.MoveTowards(ballObject.transform.position,target,3*Time.deltaTime);
                yield return null;
            }
        }   
        yield return null;
        Destroy(ballObject.gameObject);   
        bottleGraphics[command.toBottleIndex].SetGraphic(command.toBallIndex, command.type);
        pendingBalls--;

    }
}
 