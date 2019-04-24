using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScreen : MonoBehaviour
{
    public GameObject deathMenuScreen;
    bool gameOver=false;

    public void EndGame()
    {
        if(gameOver==false)
        {
            gameOver=true;
            deathMenuScreen.SetActive(true);
            Time.timeScale=0f;
        }
    }
    public void ExitToMainMenu()
    {
        Time.timeScale = 1f;
         SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex-1);
    }
}
