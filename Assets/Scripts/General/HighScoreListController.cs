using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class HighScoreListController : MonoBehaviour
{
    public TextMeshProUGUI highScoreListText;

    private void Start() {
        
        // Activated for debuging
        //HighScoreController.ResetHighScores();

        if(PlayerPrefsX.GetIntArray("HighScores") == null){
            HighScoreController.ResetHighScores();
        }
    }

    private void Update() {
        if(PlayerPrefsX.GetIntArray("HighScores") != null){
            if(PlayerPrefsX.GetIntArray("HighScores").Length > 0){
                string list = "";

                int[] highScores = PlayerPrefsX.GetIntArray("HighScores");
                string[] highScoreNames = PlayerPrefsX.GetStringArray("HighScoreNames");

                for (int i = 0; i < highScores.Length; i++)
                {
                    list += highScoreNames[i] + ": " + highScores[i] + "\n";
                }

                highScoreListText.text = list;
            }
        }
    }
}
