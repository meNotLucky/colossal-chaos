using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStartScript : MonoBehaviour
{
    public bool playStartAnimation = true;
    private Camera paningCamera;
    private Animator cameraAnimator;
    private GiantControllerV2 giant;
    private PopUpController popUps;

    bool isPlaying;

    void Start() {
        paningCamera = GetComponent<Camera>();
        cameraAnimator = GetComponent<Animator>();
        giant = FindObjectOfType<GiantControllerV2>();
        popUps = FindObjectOfType<PopUpController>();

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

    public void ActivatePopUp(string popUp){
        popUps.ActivatePopUp(popUp, 0);
    }

    public void DeactivatePopUp(string popUp){
        popUps.DeactivatePopUp(popUp);
    }

    public void PanFinished(){
        cameraAnimator.SetBool("IsStarting", false);
        isPlaying = false;
        giant.HandleStartPan(false);
    }
}
