using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class LooseCondition : MonoBehaviour
{
    public GameObject slider;
    public TextMeshProUGUI text;
    private float startTime;
    [HideInInspector] public int numberOfHouses;
    [HideInInspector] public int destroyedHouses = 0;
    [HideInInspector] public int landmarkDestructionPoints = 0;
    [HideInInspector] public float totalDestructionPoints;
    [HideInInspector] public float totalScore;

    void Start()
    {
        numberOfHouses = GameObject.FindGameObjectsWithTag("house").Length;
        startTime = GlobalSettings.maxDestroyedHouses;
    }

    void Update()
    {
        slider.GetComponent<SliderScript>().SetMinMaxValue(0, startTime);
        destroyedHouses = numberOfHouses - GameObject.FindGameObjectsWithTag("house").Length;

        totalDestructionPoints = destroyedHouses + landmarkDestructionPoints;
        totalScore = startTime - totalDestructionPoints - Time.timeSinceLevelLoad;

        slider.GetComponent<SliderScript>().SetValue(totalScore);

        float minutes = Mathf.Floor(totalScore / 60);
        float seconds = Mathf.RoundToInt(totalScore % 60);

        float minutesToShow = minutes; float secondsToShow = seconds;

        if(secondsToShow > 59.0f) {
            minutesToShow = minutes + 1.0f;
            secondsToShow = 0.0f;
        } else {
            minutesToShow = minutes;
            secondsToShow = seconds;
        }

        string secondsText = seconds.ToString();
        string minutesText = minutes.ToString();

        if(seconds < 10.0f)
            secondsText = "0" + seconds.ToString();
        if(minutes < 10.0f)
            minutesText = "0" + minutes.ToString();
        
        text.text = minutesText.ToString() + ":" + secondsText.ToString();
                
        if(totalDestructionPoints >= GlobalSettings.maxDestroyedHouses)
        {
            if(FindObjectOfType<DeathScreen>() != null)
                FindObjectOfType<DeathScreen>().EndGame();
        }  
        
    }
}
