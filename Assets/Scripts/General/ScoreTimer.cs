using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class ScoreTimer : MonoBehaviour
{
    public GameObject slider;
    public TextMeshProUGUI text;
    private float startTime;
    private float timeSinceLevelLoad;

    [HideInInspector] public bool timerStart = false;
    [HideInInspector] public int numberOfHouses;
    [HideInInspector] public int destroyedHouses = 0;
    [HideInInspector] public int landmarkDestructionPoints = 0;
    [HideInInspector] public float totalDestructionPoints;
    [HideInInspector] public float totalScore;

    void Start()
    {
        numberOfHouses = GameObject.FindGameObjectsWithTag("house").Length;
        startTime = GlobalSettings.startTime;
    }

    void Update()
    {
        if(!timerStart){
            text.text = "00:00";
            timeSinceLevelLoad = Time.timeSinceLevelLoad;
        } else {
            slider.GetComponent<SliderScript>().SetMinMaxValue(0, startTime);
            destroyedHouses = numberOfHouses - GameObject.FindGameObjectsWithTag("house").Length;

            totalDestructionPoints = destroyedHouses + landmarkDestructionPoints;
            totalScore = startTime - totalDestructionPoints - (Time.timeSinceLevelLoad - timeSinceLevelLoad);

            slider.GetComponent<SliderScript>().SetValue(totalScore);            
            text.text = ConvertScoreToTimeString(totalScore);
                    
            if(totalDestructionPoints >= GlobalSettings.startTime)
            {
                if(FindObjectOfType<DeathScreen>() != null)
                    FindObjectOfType<DeathScreen>().EndGame();
            }
        }
    }

    public static string ConvertScoreToTimeString(float score){

        float minutes = Mathf.Floor(score / 60);
        float seconds = Mathf.RoundToInt(score % 60);
        float minutesToShow = minutes; float secondsToShow = seconds;

        if(secondsToShow > 59.0f) {
            minutesToShow = minutes + 1.0f;
            secondsToShow = 0.0f;
        } else {
            minutesToShow = minutes;
            secondsToShow = seconds;
        }

        string secondsText = secondsToShow.ToString();
        string minutesText = minutesToShow.ToString();


        if(secondsToShow < 10.0f)
            secondsText = "0" + secondsToShow.ToString();
        if(minutesToShow < 10.0f)
            minutesText = "0" + minutesToShow.ToString();
        
        return minutesText.ToString() + ":" + secondsText.ToString();
    }
}
