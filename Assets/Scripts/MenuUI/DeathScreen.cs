using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeathScreen : MonoBehaviour
{
    public GameObject deathMenuScreen;
    bool gameOver = false;
    public Slider ExitToMainMenuSlider;
    public float SliderSpeed = 0.1f;

    void Update()
    {
        if(gameOver == true)
        {
            Vector3 mousePos=Input.mousePosition;
            if(mousePos.x<Screen.width / 2 - 40f)
            {
                ExitToMainMenuSlider.value += SliderSpeed / 60;
            
                if(ExitToMainMenuSlider.value >= ExitToMainMenuSlider.maxValue)
                {
                ExitToMainMenu();
                }
            }
            else if(mousePos.x > (Screen.width / 2) - (Screen.width / 8))
            {
                ExitToMainMenuSlider.value -= SliderSpeed / 60;
            }
        }
        
    }
    public void EndGame()
    {
        if(gameOver == false)
        {
            gameOver = true;
            deathMenuScreen.SetActive(true);
            Time.timeScale = 0f;
        }
    }
    public void ExitToMainMenu()
    {
        Time.timeScale = 1f;
         SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 2);
         
    }
    
}
