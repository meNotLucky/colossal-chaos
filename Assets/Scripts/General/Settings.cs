using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    [Header("Gameplay Settings")]
    public float maxDestroyedHouses = 100;
    public int numberOfSavedHighScores = 10;

    [Header("Performance Settings")]
    public float debrieLifeDuration = 5;

    private void Awake(){
        GlobalSettings.maxDestroyedHouses = maxDestroyedHouses;
        GlobalSettings.debrieLifeDuration = debrieLifeDuration;
        GlobalSettings.numberOfSavedHighScores = numberOfSavedHighScores;
    }
}
