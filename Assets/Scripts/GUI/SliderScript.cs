using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderScript : MonoBehaviour
{
    public float minimumHouseAmount;
    public Slider DestructionMeter;
    
    void Start()
    {
        DestructionMeter.maxValue = GameObject.FindGameObjectsWithTag("house").Length;
        DestructionMeter.minValue = minimumHouseAmount;  
    }

    
    void Update()
    {
        DestructionMeter.value = GameObject.FindGameObjectsWithTag("house").Length; 
        //Debug.Log(DestructionMeter.value );    
        if(DestructionMeter.value<=minimumHouseAmount)
        {
            FindObjectOfType<DeathScreen>().EndGame();
        }  
    }
}
