using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    [Header("Gameplay Settings")]
    public float minimumHouseAmount;

    [Header("Performance Settings")]
    public float debrieLifeDuration;

    private void Update(){
        GlobalSettings.minimumHouseAmount = minimumHouseAmount;
        GlobalSettings.debrieLifeDuration = debrieLifeDuration;
    }
}
