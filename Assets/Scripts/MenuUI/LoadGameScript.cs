using System.Collections;
using UnityEngine;

using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LoadGameScript : MonoBehaviour
{
    public GameObject loadingScreen;
    public Slider slider;
    public TextMeshProUGUI progressionText;

    public void LoadGame(){
        loadingScreen.SetActive(true);
        StartCoroutine(LoadGameProcess());
    }
    
    IEnumerator LoadGameProcess(){

        AsyncOperation operation = SceneManager.LoadSceneAsync(1);

        while(!operation.isDone){
            float progress = Mathf.Clamp01(operation.progress / .9f);
            slider.value = progress;
            progressionText.text = progress * 100 + "%";

            yield return null;
        }
    }
}
