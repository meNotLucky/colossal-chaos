using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPanScript : MonoBehaviour
{
    public bool playStartAnimation = true;
    private Camera paningCamera;
    private Animator cameraAnimator;
    private GiantControllerV2 giant;
    private PopUpController popUps;
    private ScoreTimer scoreTime;

    bool isPlaying = false;
    private Vector3 originalCamPosition;
    private Vector3 originalCamRotation;
    private float originalLodBias;

    void Start() {
        paningCamera = GetComponent<Camera>();
        cameraAnimator = GetComponent<Animator>();
        giant = FindObjectOfType<GiantControllerV2>();
        popUps = FindObjectOfType<PopUpController>();
        scoreTime = FindObjectOfType<ScoreTimer>();
        
        cameraAnimator.enabled = true;

        originalCamPosition = paningCamera.transform.localPosition;
        originalCamRotation = paningCamera.transform.localEulerAngles;
        originalLodBias = QualitySettings.lodBias;

        if(playStartAnimation){
            cameraAnimator.SetBool("IsStarting", true);
            isPlaying = true;
            scoreTime.timerStart = false;
            QualitySettings.lodBias = 20;
        } else {
            PanFinished();
        }
    }

    private void Update() {
        if(isPlaying)
            giant.HandleStartPan(true);
        
        if(Input.GetButtonDown("SideStep Right") && isPlaying)
            PanFinished();
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
        scoreTime.timerStart = true;
        QualitySettings.lodBias = originalLodBias;
        cameraAnimator.enabled = false;
        paningCamera.transform.localPosition = originalCamPosition;
        paningCamera.transform.localEulerAngles = originalCamRotation;
        popUps.DeactivateAllPopUps();
        giant.HandleStartPan(false);
    }
}
