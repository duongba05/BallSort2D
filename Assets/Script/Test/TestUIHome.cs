using UnityEngine;

public class TestUIHome : MonoBehaviour
{
    public UIHome ui;
    int hintCount = 5;
    private void Start()
    {
        ui.SetHintCount(1);

        ui.SetRedoCount(3);

        ui.SetLevelText(12);
        ui.OnClickButtonHint = OnCLickButtonHint;
    }
    private void OnCLickButtonHint()
    {
        Debug.Log("Click Hint!");
        if(hintCount > 0)
        {
            hintCount--;
            ui.SetHintCount(hintCount);
        }
    }

}
