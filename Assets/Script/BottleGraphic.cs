using UnityEngine;
using UnityEngine.UIElements;

public class BottleGraphic : MonoBehaviour
{
    public int index;
    public BallGraphic[] ballGraphics;
    private void OnMouseUpAsButton()
    {
        Debug.Log("Click" + index);
    }
    public void SetGraphic(Game.BallType[] ballTypes)
    {
        for(int i = 0; i<ballGraphics.Length ; i++)
        {
            if (i >= ballTypes.Length)
            {
                ballGraphics[i].SetColor(BallGraphicType.NONE);
            }
            else
            {
                BallGraphicType type = BallGraphicType.RED ;
                switch (ballTypes[i])
                {
                    case Game.BallType.RED: 
                        type = BallGraphicType.RED;
                        break;
                    case Game.BallType.GREEN:
                        type = BallGraphicType.GREEN;
                        break;
                }
                ballGraphics[i].SetColor(type);
            }
        }
    }
}
