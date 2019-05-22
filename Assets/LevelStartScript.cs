using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStartScript : MonoBehaviour
{
    private Camera paningCamera;
    private Animator cameraAnimator;
    private GiantControllerV2 giant;

    bool isPlaying = true;

    void Start() {
        paningCamera = GetComponent<Camera>();
        cameraAnimator = GetComponent<Animator>();
        giant = FindObjectOfType<GiantControllerV2>();
        cameraAnimator.SetBool("IsStarting", true);
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
