using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("Level");
    }
    public void LikeGame()
    {
        Debug.Log("Like game click");
    }
    public void ShareGame()
    {
        Debug.Log("ShareGame game click");
    }
}
