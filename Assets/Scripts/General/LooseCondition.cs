using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LooseCondition : MonoBehaviour
{
    public GameObject slider;
    [HideInInspector] public int numberOfHouses;
    [HideInInspector] public int destroyedHouses = 0;
    [HideInInspector] public int landmarkDestructionPoints = 0;

    void Start()
    {
        numberOfHouses = GameObject.FindGameObjectsWithTag("house").Length;
    }

    void Update()
    {
        slider.GetComponent<SliderScript>().SetMinMaxValue(0, GlobalSettings.maxDestroyedHouses);
        destroyedHouses = numberOfHouses - GameObject.FindGameObjectsWithTag("house").Length;

        slider.GetComponent<SliderScript>().SetValue(destroyedHouses + landmarkDestructionPoints);

        if(destroyedHouses >= GlobalSettings.maxDestroyedHouses)
        {
            if(FindObjectOfType<DeathScreen>() != null)
                FindObjectOfType<DeathScreen>().EndGame();
        }  
        
    }
}
