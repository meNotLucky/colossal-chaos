using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class HighScoreController : MonoBehaviour
{
    [Header("High Score Properties")]
    public TextMeshProUGUI scoreTextWinScreen;
    
    [Header("High Score Values")]
    [SerializeField] float currentScore;

    private ScoreTimer scoreTime;

    private void Start() {
        
        // Activated for debuging
        // ResetHighScores();

        scoreTime = FindObjectOfType<ScoreTimer>();

        if(PlayerPrefsX.GetFloatArray("HighScores") == null){
            ResetHighScores();
        }
    }

    private void Update() {
        currentScore = scoreTime.totalScore;
        scoreTextWinScreen.text = "Final Score: " + ScoreTimer.ConvertScoreToTimeString(currentScore);
    }

    public void SaveScore(string name) {

        if(name.Length <= 1)
            return;

        if(PlayerPrefsX.GetFloatArray("HighScores") == null){
            ResetHighScores();
        }

        float[] highScores = PlayerPrefsX.GetFloatArray("HighScores");
        List<float> highScoreList = new List<float>(highScores);

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
        PlayerPrefsX.SetFloatArray("HighScores", highScores);
        PlayerPrefsX.SetStringArray("HighScoreNames", highScoreNames);

        Debug.Log("Saved");
    }

    public static void ResetHighScores(){
        float[] highScores = new float[GlobalSettings.numberOfSavedHighScores];
        string[] highScoreNames = new string[GlobalSettings.numberOfSavedHighScores];

        for (int i = 0; i < GlobalSettings.numberOfSavedHighScores; i++)
        {
            highScores[i] = 0;
            highScoreNames[i] = "N/A";
        }

        PlayerPrefsX.SetFloatArray("HighScores", highScores);
        PlayerPrefsX.SetStringArray("HighScoreNames", highScoreNames);
    }
}
