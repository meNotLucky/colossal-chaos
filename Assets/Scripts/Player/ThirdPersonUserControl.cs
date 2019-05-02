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

    [Header("Player Input Data")]
    public float mouseInputSensitivity;
    public float sensitivityModifier;

    [Header("Giant AI Data")]
    public float giantTurnSpeed;
    public float struggleModifier;
    public float struggleIntensityMax;
    public float struggleIntensityMin;
    private bool sensModified = true;
    public List<GameObject> targets = new List<GameObject>();
    public GameObject closestTarget;
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

        if(!stop)
            stop = Input.GetButtonDown("Stop");

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
        if(closestTarget != null){
            Vector3 targetDir = (closestTarget.transform.position - transform.position).normalized;
            lookRotation = Quaternion.LookRotation(targetDir);
        }

        // calculate move direction to pass to character
        if (cam != null){
            if(closestTarget != null){
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * giantTurnSpeed);
                transform.rotation = Quaternion.Euler(new Vector3(0f, transform.rotation.eulerAngles.y, 0f));
                move = (v * camForward) + (h * cam.right);
            } else {
                move = (v * camForward) + (h * cam.right);
            }
        }
        else{
            // we use world-relative directions in the case of no main camera
            move = (v * Vector3.forward) + (h * Vector3.right);
        }

        // pass all parameters to the character control script
        character.Move(move, jump, stop);
        jump = false; stop = false;
    }

    private void UpdateTarget() {
        if(targets.Count > 0){
            closestTarget = targets[0];
            float currentTargetDist = Vector3.Distance(closestTarget.transform.position, transform.position);
            Debug.Log(currentTargetDist);

            foreach(var target in targets){
                float targetDist = Vector3.Distance(target.transform.position, transform.position);
                if(targetDist < currentTargetDist){
                    closestTarget = target;
                }
            }
        } else {
            closestTarget = null;
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
