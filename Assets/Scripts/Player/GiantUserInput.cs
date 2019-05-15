using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GiantController))]
public class GiantUserInput : MonoBehaviour
{
    private GiantController character; // A reference to the ThirdPersonCharacter on the object
    private Transform cam; // A reference to the main camera in the scenes transform
    private Vector3 move; // the world-relative desired move direction, calculated from the camera and user input.
    private bool jump;
    private bool stop;
    private bool sideStepLeft;
    private bool sideStepRight;

    [Header("Player Input Data")]
    public float mouseInputSensitivity;
    public float sensitivityModifier;
    public float mouseCenterOffset;

    [Header("Giant AI Data")]
    public float giantPullForce;
    public float distanceToRotate;
    public float struggleModifier;
    public float struggleIntensityMax;
    public float struggleIntensityMin;
    private bool sensModified = true;
    public List<AttractionTarget> targets;
    public GameObject currentTarget;
    private Quaternion lookRotation;

    private SoundEffectManager soundEffect;

    [HideInInspector] public float h;
    private float v = 1;

    
    private void Start()
    {
        // Get targets
        targets = new List<AttractionTarget>(FindObjectsOfType<AttractionTarget>());
      
        // Put this here cause I don't know where else to put it right now
        FindObjectOfType<PopUpController>().ActivatePopUp("Start", 6);

        // Center mouse
        Cursor.lockState = CursorLockMode.Locked;

        // get the transform of the main camera
        if (Camera.main != null)
            cam = Camera.main.transform;
        else
            Debug.LogWarning("Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.", gameObject);

        // get the third person character ( this should never be null due to require component )
        character = GetComponent<GiantController>();
    }

    private void Update()
    {
        // Release mouse when moved
        if(Input.GetAxisRaw("Mouse X") != 0)
            Cursor.lockState = CursorLockMode.None;

            

        // Read inputs
        // if(!jump)
        //     jump = Input.GetButtonDown("Jump");

        if(!stop && !sideStepLeft && !sideStepRight){
            if(Input.GetKey(KeyCode.X) && Input.GetKey(KeyCode.Y))
                stop = true;
        }

        if(!sideStepLeft && !stop)
            if(Input.GetButton("SideStep Left")){
                sideStepLeft = true;
            }

        if(!sideStepRight && !sideStepLeft && !stop)
            if(Input.GetButton("SideStep Right")){
                sideStepRight = true;
            }

        h = - (Input.mousePosition.x - ((Screen.width / 2)) + mouseCenterOffset) * (mouseInputSensitivity / sensitivityModifier);

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
                move = (v * camForward) + (h * cam.right);
            }
            else {
                move = (v * camForward) + (h * cam.right);
            }
        }
        else{
            // we use world-relative directions in the case of no main camera
            move = (v * Vector3.forward) + (h * Vector3.right);
        }

        // pass all parameters to the character control script
        character.Move(move, jump, stop, sideStepLeft, sideStepRight);
        jump = false; stop = false; sideStepLeft = false; sideStepRight = false;
    }

    private void UpdateTarget() {

        if(targets.Count > 0){
            foreach(var target in targets){
                float targetDist = Vector3.Distance(target.transform.position, transform.position);
                if(targetDist < target.GetComponent<AttractionTarget>().GetRange())
                    currentTarget = target.gameObject;
            }
            if(currentTarget != null){
                Vector3 targetDir = currentTarget.transform.position - transform.position;
                Vector3 forward = transform.forward;
                float angle = Vector3.SignedAngle(targetDir, forward, Vector3.up);

                if(angle < 0){
                    if(FindObjectOfType<PopUpController>() != null){
                        FindObjectOfType<PopUpController>().DeactivatePopUp("ChurchLeft");
                        FindObjectOfType<PopUpController>().ActivatePopUp("ChurchRight", 0);
                    }
                } else {
                    if(FindObjectOfType<PopUpController>() != null){
                        FindObjectOfType<PopUpController>().DeactivatePopUp("ChurchRight");
                        FindObjectOfType<PopUpController>().ActivatePopUp("ChurchLeft", 0);
                    }
                }
                float currentTargetDistance = Vector3.Distance(currentTarget.transform.position, transform.position);
                if(currentTargetDistance > currentTarget.GetComponent<AttractionTarget>().GetRange())
                    currentTarget = null;                    
            }
            if(currentTarget == null){
                if(FindObjectOfType<PopUpController>() != null){
                    FindObjectOfType<PopUpController>().DeactivatePopUp("ChurchRight");
                    FindObjectOfType<PopUpController>().DeactivatePopUp("ChurchLeft");
                }
            }
        }
    }

    public void RemoveCurrentTarget() {
        targets.Remove(currentTarget.GetComponent<AttractionTarget>());
        currentTarget = null;
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

