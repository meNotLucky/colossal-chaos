using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class HighScoreController : MonoBehaviour
{
    [Header("High Score Properties")]
    public int startScore;
    public TextMeshProUGUI scoreTextPlaying;
    public TextMeshProUGUI scoreTextWinScreen;
    
    [Header("High Score Values")]
    [SerializeField] int currentScore;

    private void Start() {
        
        // Activated for debuging
        // ResetHighScores();

        if(PlayerPrefsX.GetIntArray("HighScores") == null){
            ResetHighScores();
        }
    }

    private void Update() {
        currentScore = startScore - FindObjectOfType<LooseCondition>().destroyedHouses;
        scoreTextPlaying.text = "Score: " + currentScore;
        scoreTextWinScreen.text = "Final Score: " + currentScore;
    }

    public void SaveScore(string name) {

        if(name.Length <= 1)
            return;

        if(PlayerPrefsX.GetIntArray("HighScores") == null){
            ResetHighScores();
        }

        int[] highScores = PlayerPrefsX.GetIntArray("HighScores");
        List<int> highScoreList = new List<int>(highScores);

        string[] highScoreNames = PlayerPrefsX.GetStringArray("HighScoreNames");
        List<string> highScoreNameList = new List<string>(highScoreNames);

        foreach(int score in highScoreList){
            if(currentScore >= score){
                int index = highScoreList.IndexOf(score);
                highScoreList.Insert(index, currentScore);
                highScoreNameList.Insert(index, name);
                highScoreList.RemoveAt(highScoreList.Count - 1);
                highScoreNameList.RemoveAt(highScoreNameList.Count - 1);
                break;
            }
        }

        highScores = highScoreList.ToArray();
        highScoreNames = highScoreNameList.ToArray();
        PlayerPrefsX.SetIntArray("HighScores", highScores);
        PlayerPrefsX.SetStringArray("HighScoreNames", highScoreNames);

        Debug.Log("Saved");
    }

    public static void ResetHighScores(){
        int[] highScores = new int[GlobalSettings.numberOfSavedHighScores];
        string[] highScoreNames = new string[GlobalSettings.numberOfSavedHighScores];

        for (int i = 0; i < GlobalSettings.numberOfSavedHighScores; i++)
        {
            highScores[i] = 0;
            highScoreNames[i] = "NaN";
        }

        PlayerPrefsX.SetIntArray("HighScores", highScores);
        PlayerPrefsX.SetStringArray("HighScoreNames", highScoreNames);
    }
}
