using UnityEngine;

public class BallGraphic : MonoBehaviour
{
    public SpriteRenderer spriteRenderer; 
    public Sprite[] sprites;// mảng màu bóng khác nhau 

    private void Awake()
    {
        spriteRenderer =GetComponent<SpriteRenderer>();
    }
    public void SetColor(int type)
    {
        if(type == 0)
        {
            spriteRenderer.color = new Color(0,0,0,0); // set không hiển thị 
        }
        else
        {
            int index = type - 1; // trừ đi 1 để trùng với index mảng 
            spriteRenderer.color = Color.white; 
            spriteRenderer.sprite = sprites[index]; // gán sprite tại index cho nó 
        }
    }
}
