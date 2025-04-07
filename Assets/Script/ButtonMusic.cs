using UnityEngine;
using UnityEngine.UI;

public class ButtonMusic : MonoBehaviour
{
    public GameObject musicOnButtonObj;
    public GameObject musicOffButtonObj;

    public Button musicOnButton;
    public Button musicOffButton;
    private void Start()
    {
        musicOnButton.onClick.AddListener(() => ToggleMusic(false));
        musicOffButton.onClick.AddListener(() => ToggleMusic(true));
    }
    void ToggleMusic(bool isOn)
    {
        if (MusicManager.Instance == null)
        {
            Debug.LogError("MusicManager.Instance is null!");
            return;
        }

        if (isOn == false)
        {
            musicOffButtonObj.SetActive(true);
            musicOnButtonObj.SetActive(false);
        }
        else
        {
            musicOffButtonObj.SetActive(false);
            musicOnButtonObj.SetActive(true);
        }
        Debug.Log("Toggling music: " + (isOn ? "On" : "Off"));
        MusicManager.Instance.isMusicOn = isOn;
        Debug.Log("isMusicOn is now: " + MusicManager.Instance.isMusicOn);
        MusicManager.Instance.UpdateMusic();
    }
}
