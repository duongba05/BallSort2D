using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    [SerializeField] private GameObject menuDialog;
    [SerializeField] private GameObject howToPlayDialog;
    [SerializeField] private GameObject completeDialog;

    [SerializeField] private Button menuBtn;
    [SerializeField] private Button retryBtn;
    [SerializeField] private Button nextBtn;

    [SerializeField] private GameObject winFX;

    [SerializeField] private Button closeHowToPlay;
    [SerializeField] private Button closeMenu;
    void Start()
    {

        menuBtn.onClick.AddListener(ShowMenuDialog);
        retryBtn.onClick.AddListener(RestartGame);
        nextBtn.onClick.AddListener(NextLevel);

        closeHowToPlay.onClick.AddListener(HideHowToPlayDialog);
        closeMenu.onClick.AddListener(HideMenuDialog);
    }

    public void ShowMenuDialog()
    {
        menuDialog.SetActive(true);
        Time.timeScale = 0f;
    }
    public void HideMenuDialog()
    {
        menuDialog.SetActive(false);
        Time.timeScale = 1f;
    }
    public void ShowCompleteialog()
    {
        winFX.SetActive(true);
        completeDialog.SetActive(true);
    }
    public void HideCompleteDialog()
    {
        completeDialog.SetActive(false);
    }
    public void ShowHowToPlayDialog()
    {
        howToPlayDialog.SetActive(true);    
    }
    public void HideHowToPlayDialog()
    {
        howToPlayDialog.SetActive(false);
    }
    private void NextLevel()
    {
        int currentLevel = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentLevel + 1);
    }
    private void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
