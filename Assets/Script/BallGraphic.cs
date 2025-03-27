using UnityEngine;

public enum BallGraphicType
{
    NONE,
    RED,
    GREEN
}
public class BallGraphic : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    private void Start()
    {
        spriteRenderer =GetComponent<SpriteRenderer>();
    }
    public void SetColor(BallGraphicType type)
    {
        switch (type)
        {
            case BallGraphicType.NONE:
                spriteRenderer.color = new Color(0,0,0,0);
                break;
            case BallGraphicType.RED:
                spriteRenderer.color = Color.red;
                break;
            case BallGraphicType.GREEN:
                spriteRenderer.color = Color.green;
                break;
        }
    }
    public static BallGraphicType ConvertFromGameType(Game.BallType ballType)
    {
        switch (ballType)
        {
            case Game.BallType.RED:
                return BallGraphicType.RED;
            case Game.BallType.GREEN:
                return BallGraphicType.GREEN;
        }
        return BallGraphicType.NONE;
    }
}
