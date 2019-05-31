using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    [Header("Gameplay Settings")]
    public float startTime = 100;
    public int numberOfSavedHighScores = 10;
    public bool showCursor = false;

    [Header("Performance Settings")]
    public float debrieLifeDuration = 5;

    private void Awake(){
        GlobalSettings.startTime = startTime;
        GlobalSettings.debrieLifeDuration = debrieLifeDuration;
        GlobalSettings.numberOfSavedHighScores = numberOfSavedHighScores;

        Cursor.visible = showCursor;
    }
}
