using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class TextFlash : MonoBehaviour
{
    public Color flashColor;
    public int timesToFlash;
    public float flashSpeed;

    private Color startColor;
    private Color currentColor;
    private int timesFlashed;

    bool flashFinished = true;
    
    TextMeshProUGUI tmp;
    Coroutine coroutine;
    

    void Start()
    {
        if(GetComponent<TextMeshProUGUI>() != null){
            tmp = GetComponent<TextMeshProUGUI>();
            startColor = tmp.color;
            currentColor = startColor;
        }
    }

    void FixedUpdate()
    {
        tmp.color = currentColor;

        if(flashFinished){
            if(coroutine != null)
                StopCoroutine(coroutine);
            tmp.color = startColor;
        }
    }

    public void Flash()
    {
        if(flashFinished){
            timesFlashed = 0;
            flashFinished = false;
            coroutine = StartCoroutine(InitializeFlash());
        }
    }

    IEnumerator InitializeFlash(){
        while(true){
            if(timesFlashed < timesToFlash){
                if(currentColor == startColor)
                    currentColor = flashColor;
                else if(currentColor == flashColor)
                    currentColor = startColor;
            }
            else {
                timesFlashed = timesToFlash;
                flashFinished = true;
                yield return null;
            }

            timesFlashed++;
            yield return new WaitForSeconds(1 / flashSpeed);
        }
    }
}
