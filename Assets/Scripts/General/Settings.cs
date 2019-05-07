using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    [Header("Gameplay Settings")]
    public float maxDestroyedHouses;

    [Header("Performance Settings")]
    public float debrieLifeDuration;

    private void Awake(){
        GlobalSettings.maxDestroyedHouses = maxDestroyedHouses;
        GlobalSettings.debrieLifeDuration = debrieLifeDuration;
    }
}
