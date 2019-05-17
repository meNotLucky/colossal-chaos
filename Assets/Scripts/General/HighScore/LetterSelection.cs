using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class LetterSelection : MonoBehaviour
{
    public TextMeshProUGUI[] displayLetters;
    public float scrollSpeed;
    public bool isActiveSelector = false;

    [HideInInspector] public char[] alphabet = {'A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','X','Y'};
    [HideInInspector] public int currentlySelected = 0;

    private bool scrolling = false;
    private Coroutine coroutine;

    private void Start(){
        UpdateLetters(0);
        StartCoroutine(LetterScroll(1, scrollSpeed));
    }

    private void Update() {
        foreach (var img in GetComponentsInChildren<Image>()){
            img.enabled = false;
            if(img.gameObject.GetComponentInChildren<TextMeshProUGUI>() != null)
                img.gameObject.GetComponentInChildren<TextMeshProUGUI>().enabled = false;
        }

        if(isActiveSelector){
            foreach (var img in GetComponentsInChildren<Image>()){
                img.enabled = true;
                if(img.gameObject.GetComponentInChildren<TextMeshProUGUI>() != null)
                    img.gameObject.GetComponentInChildren<TextMeshProUGUI>().enabled = true;
            }
        }
    }

    private void UpdateLetters(int direction){

        if(direction > 0)
            currentlySelected++;
        else if(direction < 0)
            currentlySelected--;

        if(currentlySelected < 0)
            currentlySelected = alphabet.Length - 1;
        if(currentlySelected > alphabet.Length - 1)
            currentlySelected = 0;

        if(currentlySelected == 0){
            displayLetters[0].text = alphabet[alphabet.Length - 1].ToString();
            displayLetters[1].text = alphabet[currentlySelected].ToString();
            displayLetters[2].text = alphabet[currentlySelected + 1].ToString();   
        }
        if(currentlySelected == alphabet.Length - 1){
            displayLetters[0].text = alphabet[currentlySelected - 1].ToString();
            displayLetters[1].text = alphabet[currentlySelected].ToString();
            displayLetters[2].text = alphabet[0].ToString();   
        }
        if(currentlySelected > 0 && currentlySelected < alphabet.Length - 1){
            displayLetters[0].text = alphabet[currentlySelected - 1].ToString();
            displayLetters[1].text = alphabet[currentlySelected].ToString();
            displayLetters[2].text = alphabet[currentlySelected + 1].ToString();   
        }
    }

    IEnumerator LetterScroll(int direction, float speed){

        while(true){
            if(isActiveSelector){
                float h = 0;
                if(FindObjectOfType<GiantUserInput>() != null)
                    h = FindObjectOfType<GiantUserInput>().h;
                if(FindObjectOfType<GiantUserInputV2>() != null)
                    h = FindObjectOfType<GiantUserInputV2>().h;
                

                if(h > -0.2f)
                    UpdateLetters(1);
                if(h < 0.2f)
                    UpdateLetters(-1);
            }
            
            yield return new WaitForSecondsRealtime(1 / speed);
        }
    }
}
