using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WinCondition : MonoBehaviour
{
    public GameObject WinScreenUI;
    public GameObject GUI;
    
    //private bool ended = false;
    public float SliderSpeed = 0.1f;
    public Slider MainMenuSlider;
    
    void Update()
    {
        // if(ended == true)
        // {
        //     Vector3 mousePos = Input.mousePosition;
        //     if(mousePos.x < (Screen.width / 2) - (Screen.width / 10))
        //     {
        //         MainMenuSlider.value += SliderSpeed / 60;
            
        //         if(MainMenuSlider.value >= MainMenuSlider.maxValue)
        //         SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        //     }
        //     else if(mousePos.x > (Screen.width / 2) - (Screen.width / 8))
        //     {
        //         MainMenuSlider.value -= SliderSpeed / 60;
        //     }
        // }
        
    }
    private void OnCollisionEnter(Collision other){
        if(other.gameObject.tag == "barrel")  
        {
            //ended = true;
            Time.timeScale = 0f;
            WinScreenUI.SetActive(true);
            GUI.SetActive(false);

            FindObjectOfType<PauseMenuScript>().enabled = false;
        }
    }

    public void ExitToMainMenu(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
