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

    [Header("Giant AI Data")]
    public float giantTurnSpeed;
    public List<GameObject> targets = new List<GameObject>();
    public GameObject closestTarget;
    private Quaternion lookRotation;

    private float h;
    private float v;
    
    
    private void Start()
    {
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
        if (!jump)
            jump = Input.GetButtonDown("Jump");

        // read inputs
        h = Input.GetAxis("Horizontal");
        v = 1;//Input.GetAxisRaw("Vertical");

    }

    private void FixedUpdate()
    {
        Vector3 camForward = Vector3.Scale(cam.forward, new Vector3(1, 0, 1)).normalized;

        UpdateTarget();

        // Get camera angle to selected target
        if(closestTarget != null){
            Vector3 targetDir = (closestTarget.transform.position - transform.position).normalized;
            lookRotation = Quaternion.LookRotation(targetDir);
        }

        // calculate move direction to pass to character
        if (cam != null){
            if(!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D)){
                
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
        character.Move(move, jump);
        jump = false;
    }

    private void UpdateTarget() {
        closestTarget = targets[0];
        foreach(var target in targets){
            float currentTargetDist = Vector3.Distance(closestTarget.transform.position, transform.position);
            float targetDist = Vector3.Distance(target.transform.position, transform.position);

            if(targetDist < currentTargetDist){
                closestTarget = target;
            }
        }
    }
}
