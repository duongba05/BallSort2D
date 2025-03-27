using UnityEngine;
using UnityEngine.UIElements;

public class BottleGraphic : MonoBehaviour
{
    public int index;
    public BallGraphic[] ballGraphics;
    public GameGraphic gameGraphic;
    public Transform bottleUpTransfrom;
    private void OnMouseUpAsButton()
    {
        gameGraphic.OnClickBottle(index);
    }
    public void SetGraphic(Game.BallType[] ballTypes)
    {
        for(int i = 0; i<ballGraphics.Length ; i++)
        {
            if (i >= ballTypes.Length)
            {
                SetGraphicNone(i);
            }
            else
            {
                SetGraphic(i, ballTypes[i]);
            }
        }
    }
    public void SetGraphic(int index, Game.BallType type)
    {
        BallGraphicType colorType = BallGraphicType.RED;
        switch (type)
        {
            case Game.BallType.RED:
                colorType = BallGraphicType.RED;
                break;
            case Game.BallType.GREEN:
                colorType = BallGraphicType.GREEN;
                break;
        }
        ballGraphics[index].SetColor(colorType);
    }
    public void SetGraphicNone(int index)
    {
        ballGraphics[index].SetColor(BallGraphicType.NONE);  
    }

    public Vector3 GetBallPosition(int index)
    {
        return ballGraphics[index].transform.position;
    }
    public Vector3 GetBottleUpPosition()
    {
        return bottleUpTransfrom.transform.position;
    }
}
