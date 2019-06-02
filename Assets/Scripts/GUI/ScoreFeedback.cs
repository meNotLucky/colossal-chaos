using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class ScoreFeedback : MonoBehaviour
{
    [Header("Text Flash Properties")]
    public Color flashColor;
    public int timesToFlash;
    public float flashSpeed;

    [Header("Floating Number Properties")]
    public GameObject floatObject;

    private Color startColor;
    private Color currentColor;
    private int timesFlashed;
    bool flashFinished = true;
    
    TextMeshProUGUI tmp;
    Coroutine coroutine;
    List<GameObject> floatNumbers = new List<GameObject>();
    

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

        for(int i = 0; i < floatNumbers.Count; i++)
        {
            GameObject number = floatNumbers[i];
            if(number.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Finished")){
                floatNumbers.Remove(number);
                Destroy(number);
                i--;
            }
        }

        
    }

    public void InitializeFeedback(int removedTime)
    {
        GameObject floatNumber = Instantiate(floatObject, transform);
        floatNumber.SetActive(true);
        floatNumber.GetComponent<TextMeshProUGUI>().text = "-" + removedTime.ToString() + "s";
        floatNumber.GetComponent<Animator>().SetBool("Finished", true);

        floatNumbers.Add(floatNumber);

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
