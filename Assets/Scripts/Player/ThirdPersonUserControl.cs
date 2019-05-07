using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ThirdPersonCharacter))]
public class ThirdPersonUserControl : MonoBehaviour
{
    private ThirdPersonCharacter character; // A reference to the ThirdPersonCharacter on the object
    private Transform cam; // A reference to the main camera in the scenes transform
    private Vector3 move; // the world-relative desired move direction, calculated from the camera and user input.
    private bool jump;
    private bool stop;
    private bool sideStepLeft;
    private bool sideStepRight;

    [Header("Player Input Data")]
    public float mouseInputSensitivity;
    public float sensitivityModifier;

    [Header("Giant AI Data")]
    public float giantPullForce;
    public float distanceToRotate;
    public float struggleModifier;
    public float struggleIntensityMax;
    public float struggleIntensityMin;
    private bool sensModified = true;
    public List<GameObject> targets = new List<GameObject>();
    public GameObject currenTarget;
    private Quaternion lookRotation;

    private float h;
    private float v = 1;
    
    private void Start()
    {
        // Center mouse
        Cursor.lockState = CursorLockMode.Locked;

        // get the transform of the main camera
        if (Camera.main != null)
            cam = Camera.main.transform;
        else
            Debug.LogWarning("Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.", gameObject);

        // get the third person character ( this should never be null due to require component )
        character = GetComponent<ThirdPersonCharacter>();
    }

    private void Update()
    {
        // Release mouse when moved
        if(Input.GetAxisRaw("Mouse X") != 0)
            Cursor.lockState = CursorLockMode.None;

        // Read inputs
        if(!jump)
            jump = Input.GetButtonDown("Jump");

        if(!stop){
            if(Input.GetKey(KeyCode.X) && Input.GetKey(KeyCode.Y))
                stop = true;
        }

        if(!sideStepLeft)
            if(Input.GetKey(KeyCode.A))
                sideStepLeft = true;

        if(!sideStepRight && !sideStepLeft)
            if(Input.GetKey(KeyCode.D))
                sideStepRight = true;

        h = - (Input.mousePosition.x - (Screen.width / 2)) * (mouseInputSensitivity / sensitivityModifier);

    }

    private void FixedUpdate()
    {
        Vector3 camForward = Vector3.Scale(cam.forward, new Vector3(1, 0, 1)).normalized;

        // Targettting system
        UpdateTarget();

        // Struggle Simulation
        if(sensModified){
            sensModified = false;
            struggleModifier *= -1;
            float rand = Random.Range(struggleIntensityMin, struggleIntensityMax);
            StartCoroutine(SensitivityInterpolator(mouseInputSensitivity, mouseInputSensitivity + struggleModifier, rand));
        }

        // Get camera angle to selected target
        if(currenTarget != null){
            Vector3 targetDir = (currenTarget.transform.position - transform.position).normalized;
            lookRotation = Quaternion.LookRotation(targetDir);
        }
        
        // calculate move direction to pass to character
        if (cam != null){
            if(currenTarget != null){
                float distanceToTarget = Vector3.Distance(transform.position, currenTarget.transform.position);
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
                if(targetDist < target.GetComponent<AttractionTarget>().GetRange()){
                    currenTarget = target;
                }
            }
            if(currenTarget != null){
                float currentTargetDistance = Vector3.Distance(currenTarget.transform.position, transform.position);
                if(currentTargetDistance > currenTarget.GetComponent<AttractionTarget>().GetRange())
                    currenTarget = null;
            }
        }
    }

    public void RemoveCurrentTarget() {
        targets.Remove(currenTarget);
        currenTarget = null;
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
