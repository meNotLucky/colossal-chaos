using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStartScript : MonoBehaviour
{
    public bool playStartAnimation = true;
    private Camera paningCamera;
    private Animator cameraAnimator;
    private GiantControllerV2 giant;

    bool isPlaying;

    void Start() {
        paningCamera = GetComponent<Camera>();
        cameraAnimator = GetComponent<Animator>();
        giant = FindObjectOfType<GiantControllerV2>();

        if(playStartAnimation){
            cameraAnimator.SetBool("IsStarting", true);
            isPlaying = true;
        } else {
            PanFinished();
        }
    }

    private void Update() {
        if(isPlaying)
            giant.HandleStartPan(true);            
    }

    public void PanFinished(){
        cameraAnimator.SetBool("IsStarting", false);
        isPlaying = false;
        giant.HandleStartPan(false);
    }
}
