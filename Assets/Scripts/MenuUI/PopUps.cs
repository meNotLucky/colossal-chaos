using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopUps : MonoBehaviour
{
    public GameObject IntroPopUp;
    public GameObject text;
    public Color startColor;
    public Color endColor;
    public float speed;
    private float startTime;
    public float destroySpeed;

    void Start()
    {
        startTime = Time.time;
        IntroPopUp.SetActive(true);
        StartCoroutine(DestroyIntroPopUp());
    }
    void Update()
    {
        float t=(Mathf.Sin(Time.time*speed));
        IntroPopUp.GetComponent<Image>().color=Color.Lerp(startColor,endColor,t);
        text.GetComponent<TextMeshProUGUI>().color=Color.Lerp(startColor,endColor,t);

    }
    IEnumerator DestroyIntroPopUp()
    {
        yield return new WaitForSeconds(destroySpeed);
        IntroPopUp.SetActive(false);
    }

    
}
