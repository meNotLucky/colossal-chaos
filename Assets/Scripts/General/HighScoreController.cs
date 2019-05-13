using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class HighScoreController : MonoBehaviour
{
    [Header("High Score Properties")]
    public int startScore;
    public TextMeshProUGUI playerName;
    public TextMeshProUGUI scoreTextPlaying;
    public TextMeshProUGUI scoreTextWinScreen;

    [Header("High Score List Properties")]
    public int numberOfHighScores;
    
    [Header("High Score Values")]
    [SerializeField] int currentScore;

    private void Start() {
        // Activated for debuging
        // ResetHighScores();
    }

    private void Update() {
        currentScore = startScore - FindObjectOfType<LooseCondition>().destroyedHouses;
        scoreTextPlaying.text = "Score: " + currentScore;
        scoreTextWinScreen.text = "Final Score: " + currentScore;
    }

    public void SaveScore() {
        if(PlayerPrefsX.GetIntArray("HighScores") != null){
            if(PlayerPrefsX.GetIntArray("HighScores").Length <= 0)
            {            
                Debug.Log("New Save");

                int[] highScores = new int[numberOfHighScores];
                highScores[0] = currentScore;

                string[] highScoreNames = new string[numberOfHighScores];
                highScoreNames[0] = playerName.text;

                PlayerPrefsX.SetIntArray("HighScores", highScores);
                PlayerPrefsX.SetStringArray("HighScoreNames", highScoreNames);
            }
            else
            {
                Debug.Log("Overwrite Save");

                int[] highScores = PlayerPrefsX.GetIntArray("HighScores");
                List<int> highScoreList = new List<int>(highScores);

                string[] highScoreNames = PlayerPrefsX.GetStringArray("HighScoreNames");
                List<string> highScoreNameList = new List<string>(highScoreNames);

                foreach(int score in highScoreList){
                    if(currentScore >= score){
                        int index = highScoreList.IndexOf(score);
                        highScoreList.Insert(index, currentScore);
                        highScoreNameList.Insert(index, playerName.text);
                        highScoreList.RemoveAt(highScoreList.Count - 1);
                        highScoreNameList.RemoveAt(highScoreNameList.Count - 1);
                        break;
                    }
                }

                highScores = highScoreList.ToArray();
                highScoreNames = highScoreNameList.ToArray();
                PlayerPrefsX.SetIntArray("HighScores", highScores);
                PlayerPrefsX.SetStringArray("HighScoreNames", highScoreNames);
            }
        }
    }

    public void ResetHighScores(){
        int[] highScores = new int[numberOfHighScores];
        string[] highScoreNames = new string[numberOfHighScores];

        for (int i = 0; i < numberOfHighScores; i++)
        {
            highScores[i] = 0;
            highScoreNames[i] = "NaN";
        }

        PlayerPrefsX.SetIntArray("HighScores", highScores);
        PlayerPrefsX.SetStringArray("HighScoreNames", highScoreNames);
    }
}
