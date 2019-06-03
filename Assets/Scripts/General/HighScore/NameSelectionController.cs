using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameSelectionController : MonoBehaviour
{
    [SerializeField] public LetterSelection[] selectors;
    private int activeSelectorIndex = 0;
    string userName;

    private void Update() {
        if(Input.GetButtonDown("Right Ear"))
            activeSelectorIndex++;
        if(Input.GetButtonDown("Left Ear"))
            activeSelectorIndex--;

        if(activeSelectorIndex < 0)
            activeSelectorIndex = 0;
        
        userName = "";
        foreach (var selector in selectors){
            selector.isActiveSelector = false;
            userName += selector.GetAlphabet()[selector.currentlySelected];
        }

        if(activeSelectorIndex > selectors.Length - 1){
            activeSelectorIndex = 0;
            FindObjectOfType<HighScoreController>().SaveScore(userName);
            FindObjectOfType<WinCondition>().ExitToMainMenu();
            return;
        }

        selectors[activeSelectorIndex].isActiveSelector = true;
    }
}
