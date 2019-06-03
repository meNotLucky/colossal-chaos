using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Video;

public class TutorialController : MonoBehaviour
{
    public GameObject[] players;
    private int currentPlayer = 0;
    bool endReached = false;
    private LoadGameScript loadGameScript;

    void Start(){

        loadGameScript = FindObjectOfType<LoadGameScript>();

        foreach (var video in players){
            video.GetComponent<VideoPlayer>().isLooping = true;
        }
    }

    void Update()
    {
        if(!endReached){

            if(Input.mousePosition.x > (Screen.width / 2) + 100.0f){
                if(currentPlayer == 0)
                    currentPlayer++;
            }

            if(Input.GetButton("Right Ear") && Input.GetButton("Left Ear")){
                if(currentPlayer == 1)
                    currentPlayer++;
            }

            if(Input.GetButton("Right Eye") && Input.GetButton("Left Eye")){
                if(currentPlayer == 2)
                    currentPlayer++;
            }

            if(currentPlayer > players.Length -1){
                currentPlayer = 0;
                endReached = true;
                loadGameScript.LoadGame();
            }

            foreach (var video in players){
                if(video != players[currentPlayer]){
                    if(video.activeSelf){
                        video.SetActive(false);
                    }
                }
            }

            if(!players[currentPlayer].activeSelf){
                players[currentPlayer].SetActive(true);
            }
        }
    }
}
