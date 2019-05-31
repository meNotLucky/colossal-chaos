using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MyBox;

public class GiantUserInputV2 : MonoBehaviour
{
    private GiantControllerV2 character; // A reference to the ThirdPersonCharacter on the object
    private Transform cam; // A reference to the main camera in the scenes transform
    private bool jump;
    private bool stop;
    private bool sideStepLeft;
    private bool sideStepRight;

    [Header("Player Input Data")]
    public bool move;
    public bool rotate;
    public float mouseInputSensitivity;
    [ReadOnly] public float sensitivityModifier;
    public float mouseCenterOffset;

    [Header("Giant AI Data")]
    public float giantPullForce;
    public float distanceToRotate;
    public float struggleModifier;
    public float struggleIntensityMax;
    public float struggleIntensityMin;
    private bool sensModified = true;
    public List<AttractionTarget> targets;
    #if UNITY_EDITOR
    [ButtonMethod]
    private string GetTargets(){
        targets = new List<AttractionTarget>(FindObjectsOfType<AttractionTarget>());
        return targets.Count + " targets found on scene, cached";
    }
    #endif

    public GameObject currentTarget;
    private Quaternion lookRotation;

    private PopUpController popUpController;

    [HideInInspector] public float h;

    
    private void Start()
    {
        // Get targets
        targets = new List<AttractionTarget>(FindObjectsOfType<AttractionTarget>());

        popUpController = FindObjectOfType<PopUpController>();

        // Center mouse
        //Cursor.lockState = CursorLockMode.Locked;

        // get the transform of the main camera
        if (Camera.main != null)
            cam = Camera.main.transform;
        else
            Debug.LogWarning("Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.", gameObject);

        // get the third person character ( this should never be null due to require component )
        character = GetComponent<GiantControllerV2>();
    }

    private void Update()
    {
        // Release mouse when moved
        //if(Input.GetAxisRaw("Mouse X") != 0)
            //Cursor.lockState = CursorLockMode.None;

        if(!stop && !sideStepLeft && !sideStepRight){
            if(Input.GetButton("Right Eye") && Input.GetButton("Left Eye"))
                stop = true;
        }

        if(!sideStepLeft && !stop)
            if(Input.GetButton("Left Ear")){
                sideStepRight = true;
            }

        if(!sideStepRight && !sideStepLeft && !stop)
            if(Input.GetButton("Right Ear")){
                sideStepLeft = true;
            }

        if(rotate)
            h = - (Input.mousePosition.x - (Screen.width / 2) + mouseCenterOffset) * (mouseInputSensitivity / sensitivityModifier);
        else
            h = 0;

        // Targettting system
        UpdateTarget();
    }

    private void FixedUpdate()
    {
        Vector3 camForward = Vector3.Scale(cam.forward, new Vector3(1, 0, 1)).normalized;

        // Struggle Simulation
        if(sensModified){
            sensModified = false;
            struggleModifier *= -1;
            float rand = Random.Range(struggleIntensityMin, struggleIntensityMax);
            StartCoroutine(SensitivityInterpolator(mouseInputSensitivity, mouseInputSensitivity + struggleModifier, rand));
        }

        // Get camera angle to selected target
        if(currentTarget != null){
            Vector3 targetDir = (currentTarget.transform.position - transform.position).normalized;
            lookRotation = Quaternion.LookRotation(targetDir);
        }
        
        // calculate move direction to pass to character
        if (cam != null){
            if(currentTarget != null){
                float distanceToTarget = Vector3.Distance(transform.position, currentTarget.transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, (Time.deltaTime * giantPullForce) / (distanceToTarget > distanceToRotate ? distanceToTarget : distanceToRotate));
                transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
            }
        }

        // pass all parameters to the character control script
        character.Move(h, move, stop, sideStepLeft, sideStepRight);
        stop = false; sideStepLeft = false; sideStepRight = false;
    }

    public void SetTarget(GameObject target){
        currentTarget = target;
    }

    private void UpdateTarget() {

        if(targets.Count > 0){
            if(currentTarget != null){
                
                Vector3 targetDir = currentTarget.transform.position - transform.position;
                Vector3 forward = transform.forward;
                float angle = Vector3.SignedAngle(targetDir, forward, Vector3.up);

                if(angle < 0){
                    if(popUpController != null){
                        popUpController.DeactivatePopUp("ChurchLeft");
                        popUpController.ActivatePopUp("ChurchRight", 0);
                    }
                } else {
                    if(popUpController != null){
                        popUpController.DeactivatePopUp("ChurchRight");
                        popUpController.ActivatePopUp("ChurchLeft", 0);
                    }
                }
                float currentTargetDistance = Vector3.Distance(currentTarget.transform.position, transform.position);
                if(currentTargetDistance > currentTarget.GetComponent<AttractionTarget>().GetRange())
                    currentTarget = null;                    
            }
            if(currentTarget == null){
                if(popUpController != null){
                    popUpController.DeactivatePopUp("ChurchRight");
                    popUpController.DeactivatePopUp("ChurchLeft");
                }
            }
        }
    }

    public void RemoveCurrentTarget() {
        if(currentTarget != null){
            targets.Remove(currentTarget.GetComponent<AttractionTarget>());
            currentTarget = null;
        }
    }

    IEnumerator SensitivityInterpolator(float startValue, float endValue, float duration)
    {
        float elapsed = 0.0f;
        while (elapsed < duration)
        {
            mouseInputSensitivity = Mathf.Lerp(startValue, endValue, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        mouseInputSensitivity = mouseInputSensitivity + struggleModifier;
        sensModified = true;
    }
}

