using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameSelectionController : MonoBehaviour
{
    [SerializeField] public LetterSelection[] selectors;
    private int activeSelectorIndex = 0;
    string userName;

    private void Update() {
        if(Input.GetButtonDown("SideStep Right"))
            activeSelectorIndex++;
        if(Input.GetButtonDown("SideStep Left"))
            activeSelectorIndex--;

        if(activeSelectorIndex < 0)
            activeSelectorIndex = 0;

        userName = "";
        foreach (var selector in selectors){
            selector.isActiveSelector = false;
            userName += selector.alphabet[selector.currentlySelected];
        }
        
        if(activeSelectorIndex > selectors.Length - 1){
            activeSelectorIndex = selectors.Length - 1;
            FindObjectOfType<HighScoreController>().SaveScore(userName);
            FindObjectOfType<WinCondition>().ExitToMainMenu();
        }

        selectors[activeSelectorIndex].isActiveSelector = true;
    }
}
