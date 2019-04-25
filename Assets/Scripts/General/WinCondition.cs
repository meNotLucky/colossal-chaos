using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinCondition : MonoBehaviour
{
    public GameObject WinScreenUI;
  
    
    private void OnCollisionEnter(Collision other){
        if(other.gameObject.tag == "barrel")  
            
            {
                Time.timeScale=0f;
                WinScreenUI.SetActive(true);
            }
    }
}
