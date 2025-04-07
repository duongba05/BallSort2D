using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIHome : MonoBehaviour
{
    public TextMeshProUGUI text_level;
    public GameObject obj_redo;
    public TextMeshProUGUI text_redo;

    public GameObject obj_hint;
    public TextMeshProUGUI text_hint;

    public System.Action OnClickButtonSettings; 
    public System.Action OnClickButtonNextLevel;
    public System.Action OnClickButtonRestart;
    public System.Action OnClickButtonRedo;
    public System.Action OnClickButtonHint;
    public void SetLevelText(int level)
    {
        text_level.text = "LEVEL: "+ level.ToString();
    }
    public void SetRedoCount(int redoCount)
    {
        obj_redo.SetActive(redoCount>0);
        if(redoCount > 0 )
            text_redo.text = redoCount.ToString();
    }
    public void SetHintCount(int hintCount)
    {
        obj_hint.SetActive(hintCount > 0);
        if (hintCount > 0)
            text_hint.text = hintCount.ToString();
    }
    public void ClickButtonSettings()
    {
        OnClickButtonSettings?.Invoke();
    }
    public void ClickButtonNextLevel()
    {
        OnClickButtonNextLevel?.Invoke();
    }
    public void ClickButtonRestart()
    {
        OnClickButtonRestart?.Invoke();
    }
    public void ClickButtonRedo()
    {
        OnClickButtonRedo?.Invoke();
    }
    public void ClickButtonHint()
    {
        OnClickButtonHint?.Invoke();
    }

}
