using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PulsePopUp : MonoBehaviour
{
    public Color startColor;
    public Color endColor;
    public float speed;

    void Update()
    {
        float t = Mathf.Sin(Time.realtimeSinceStartup * speed);
        GetComponent<Image>().color = Color.Lerp(startColor, endColor,t);
    }
}
