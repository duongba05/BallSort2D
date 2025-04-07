using System.Collections.Generic;
using UnityEngine;

public static class Prefs
{
    private const string LevelKey = "PlayerLevel";

    public static int Level
    {
        get
        {
            return PlayerPrefs.GetInt(LevelKey, 0);
        }
        set
        {
            PlayerPrefs.SetInt(LevelKey, value); 
            PlayerPrefs.Save(); 
        }
    }
}
