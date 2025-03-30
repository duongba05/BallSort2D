using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameLevelReader : MonoBehaviour
{
    public Game game;
    public TextAsset textAsset;
    private void Start()
    {
        LoadLevel(textAsset);
    }
    public void LoadLevel(TextAsset textAsset)
    {
        string[] lines = textAsset.text.Split(new string[] { "\n", "\r" }, System.StringSplitOptions.RemoveEmptyEntries);
        Debug.Log(lines.Length);
        //foreach(var line in lines)
        //{
        //    // Debug.Log(line);
        //}
        int bottleCount = 0;
        int ballPerBottle = 0;
        List<int[]> bottleArrays = new List<int[]>();
        for (int i = 0; i < lines.Length; i++) 
        {
            string line = lines[i]; 
            Debug.Log(line);

            if (i == 0)
            {
                string[] line0Split = line.Split(new char[] {',' }, System.StringSplitOptions.RemoveEmptyEntries);
                bottleCount = int.Parse(line0Split[0]);
                ballPerBottle = int.Parse(line0Split[1]);
            }
            else
            {
                int[] convertArray = new int[4];
                for (int j = 0; j < convertArray.Length; j++)
                {
                    convertArray[j] = CharacterToInt(line[j]);
                }
                bottleArrays.Add(convertArray);
            }
        }
        game.LoadLevel(bottleArrays);
    }
    private int CharacterToInt(char c)
    {
        switch (c)
        {
            default: return 0;
            case '0': return 0;
            case '1': return 1;
            case '2': return 2;
            case '3': return 3;
            case '4': return 4;
            case '5': return 5;
            case '6': return 6;
            case '7': return 7;
            case '8': return 8;
            case '9': return 9;
            case 'A': return 10;
            case 'B': return 11;
            case 'C': return 12;
            case 'D': return 13;
            case 'E': return 14;
            case 'F': return 15;
        }
    }
}
