using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }
    public AudioSource audioSource;

    public bool isMusicOn = true;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("MusicManager instance created.");
        }
        else
        {
            Destroy(gameObject);
        }

        audioSource = GetComponent<AudioSource>();
        UpdateMusic();
    }

    public void UpdateMusic()
    {
        Debug.Log("Updating music. isMusicOn: " + isMusicOn);  // Kiểm tra trạng thái isMusicOn
        if (isMusicOn)
        {
            if (!audioSource.isPlaying)  // Kiểm tra nếu âm nhạc chưa phát
            {
                audioSource.Play();
                Debug.Log("Music started.");
            }
        }
        else
        {
            if (audioSource.isPlaying)  // Kiểm tra nếu âm nhạc đang phát
            {
                audioSource.Pause();
                Debug.Log("Music paused.");
            }
        }
    }


}
