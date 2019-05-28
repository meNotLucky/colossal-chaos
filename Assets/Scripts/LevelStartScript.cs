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

    bool isPlaying = false;
    private Vector3 originalCamPosition;
    private Vector3 originalCamRotation;

    void Start() {
        paningCamera = GetComponent<Camera>();
        cameraAnimator = GetComponent<Animator>();
        giant = FindObjectOfType<GiantControllerV2>();
        popUps = FindObjectOfType<PopUpController>();
        
        cameraAnimator.enabled = true;

        originalCamPosition = paningCamera.transform.localPosition;
        originalCamRotation = paningCamera.transform.localEulerAngles;

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
        
        if(Input.GetButtonDown("SideStep Right") && isPlaying){
            cameraAnimator.enabled = false;
            paningCamera.transform.localPosition = originalCamPosition;
            paningCamera.transform.localEulerAngles = originalCamRotation;
            popUps.DeactivateAllPopUps();
            PanFinished();
        }
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
