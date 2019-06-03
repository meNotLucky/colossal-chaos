using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WinCondition : MonoBehaviour
{
    public GameObject WinScreenUI;
    public GameObject HighScoreUI;
    public GameObject GUI;
    public GameObject continueText;

    private ScoreTimer scoreTimer;

    private void Start() {
        scoreTimer = FindObjectOfType<ScoreTimer>();
    }
    
    private void Update() {
        if(WinScreenUI.activeSelf && !HighScoreUI.activeSelf){
            if(Input.GetButtonDown("Right Ear")){
                float[] savedScores = PlayerPrefsX.GetFloatArray("HighScores");
                foreach (var score in savedScores){
                    if(score < scoreTimer.totalScore){
                        HighScoreUI.SetActive(true);
                        continueText.SetActive(false);
                        return;
                    }
                }

                ExitToMainMenu();
            }
        }
    }
    
    private void OnCollisionEnter(Collision other){
        if(other.gameObject.tag == "barrel"){
            Time.timeScale = 0f;
            WinScreenUI.SetActive(true);
            GUI.SetActive(false);

            FindObjectOfType<PauseMenuScript>().enabled = false;
        }
    }

    public void ExitToMainMenu(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 2);
    }
}
