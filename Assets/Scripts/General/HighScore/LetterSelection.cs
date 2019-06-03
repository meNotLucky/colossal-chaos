using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;
using MyBox;

[System.Serializable]
public class LetterSelection : MonoBehaviour
{
    public TextMeshProUGUI[] displayLetters;
    public float scrollSpeed = 2;
    public bool isActiveSelector = false;

    private char[] alphabet = {'A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z'};
    [HideInInspector] public int currentlySelected = 0;

    private Coroutine coroutine;

    private void Start(){
        UpdateLetters(0);
        StartCoroutine(LetterScroll(1, scrollSpeed));
    }

    public char[] GetAlphabet(){
        return alphabet;
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
                if(Input.mousePosition.x > (Screen.width / 2) + 100.0f)
                    UpdateLetters(1);
                if(Input.mousePosition.x < (Screen.width / 2) - 100.0f)
                    UpdateLetters(-1);
            }
            
            yield return new WaitForSecondsRealtime(1 / speed);
        }
    }
}
