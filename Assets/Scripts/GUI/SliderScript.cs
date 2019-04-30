using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class SliderScript : MonoBehaviour
{
    private int numberOfHouses;
    private int destroyedHouses = 0;

    void Start()
    {
        numberOfHouses = GameObject.FindGameObjectsWithTag("house").Length;
    }

    void Update()
    {
        GetComponent<Slider>().minValue = 0;
        GetComponent<Slider>().maxValue = GlobalSettings.maxDestroyedHouses;
        destroyedHouses = numberOfHouses - GameObject.FindGameObjectsWithTag("house").Length;

        GetComponent<Slider>().value = destroyedHouses;

        if(destroyedHouses >= GlobalSettings.maxDestroyedHouses)
        {
            FindObjectOfType<DeathScreen>().EndGame();
        }  
        
    }
    
}
