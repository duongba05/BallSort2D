using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonPlayLoadScene : MonoBehaviour
{
   public void LoadScene()
    {
        SceneManager.LoadScene("LevelSelected");
    }
}
