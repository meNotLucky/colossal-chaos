using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{
  
  public UnityEngine.UI.Button myButton;
  public UnityEngine.UI.Button quitButton;
  public Slider PlaySlider;
  public float timeToChange=1f;
  public float mouseHeldTimer=0f;
  public float redC=0.1f;
  public float timer=0.1f;
  public float colortimer;

  void Update()
  {
    Vector3 mousePos=Input.mousePosition;
    if(mousePos.x>Screen.width/2)
    {
         mouseHeldTimer +=Time.deltaTime;
        //myButton.Select();
        if(redC<=1)
        {
          redC+=colortimer/60;
          
        }
        
        
      if(PlaySlider.value<=1f){
        PlaySlider.value+=timer/60;
      }

      if(PlaySlider.value>=1)
      {
        //Debug.Log("START");
      }
        if(mouseHeldTimer>timeToChange)
        {
           //PlayGame();
           
           
        }
        
    }
    else
    {
      if(redC>=0.4f)
      {
        redC-=colortimer/60;
      }
      if(PlaySlider.value>=0)
      {
        PlaySlider.value-=timer/60;
      }
      quitButton.Select();
      
      
      mouseHeldTimer=0f;

    }
   
      ColorBlock colors=myButton.colors;
      colors.normalColor=new Color(redC,0,0);
      myButton.colors=colors;
      
    
  }
  public void PlayGame()
  {
      SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
  }
  public void QuitGame()
  {
      Debug.Log("Quit!");
      Application.Quit();
  }
 
}
