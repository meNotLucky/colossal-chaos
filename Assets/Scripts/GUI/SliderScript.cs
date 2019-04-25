using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class SliderScript : MonoBehaviour
{
    void Start()
    {
        GetComponent<Slider>().maxValue = GameObject.FindGameObjectsWithTag("house").Length;
        GetComponent<Slider>().minValue = GlobalSettings.minimumHouseAmount;
    }

    
    void Update()
    {
        GetComponent<Slider>().value = GameObject.FindGameObjectsWithTag("house").Length; 
        //Debug.Log(DestructionMeter.value );    
        if(GetComponent<Slider>().value <= GlobalSettings.minimumHouseAmount)
        {
            FindObjectOfType<DeathScreen>().EndGame();
        }  
    }
}
