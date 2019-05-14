using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class HighScoreListController : MonoBehaviour
{
    public TextMeshProUGUI highScoreListText;

    private void Start() {

        if(PlayerPrefsX.GetIntArray("HighScores") == null){
            HighScoreController.ResetHighScores();
        }
    }

    private void Update() {

        if(Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.C)){
            HighScoreController.ResetHighScores();
            Debug.Log("High Score Cleared");
        }

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
