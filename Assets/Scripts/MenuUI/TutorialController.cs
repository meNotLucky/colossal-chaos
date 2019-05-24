using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Video;

public class TutorialController : MonoBehaviour
{
    public VideoPlayer[] players;
    private int currentPlayer = 0;
    bool endReached = false;
    private LoadGameScript loadGameScript;

    void Start(){

        loadGameScript = FindObjectOfType<LoadGameScript>();

        foreach (var video in players){
            video.isLooping = true;
        }
    }

    void Update()
    {
        if(!endReached){
            if(Input.GetButtonDown("SideStep Right")){
                currentPlayer++;
                if(currentPlayer > players.Length -1){
                    currentPlayer = 0;
                    endReached = true;
                    loadGameScript.LoadGame();
                }
            }

            if(!players[currentPlayer].gameObject.activeSelf){
                players[currentPlayer].gameObject.SetActive(true);
            }

            foreach (var video in players){
                if(video != players[currentPlayer]){
                    if(video.gameObject.activeSelf){
                        video.gameObject.SetActive(false);
                    }
                }
            }
        }
    }
}
