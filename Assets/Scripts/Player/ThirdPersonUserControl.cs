using System;
using UnityEngine;

[RequireComponent(typeof (ThirdPersonCharacter))]
public class ThirdPersonUserControl : MonoBehaviour
{
    private ThirdPersonCharacter m_Character; // A reference to the ThirdPersonCharacter on the object
    private Transform m_Cam; // A reference to the main camera in the scenes transform
    private Vector3 m_Move; // the world-relative desired move direction, calculated from the camera and user input.
    private bool m_Jump;
    private Vector3 hover;

    [Header("Giant AI Data")]
    public float giantTurnForce;
    public GameObject[] targets;
    public GameObject closestTarget;
    public float targetAngle;

    [SerializeField]
    float h;
    float v;
    
    
    private void Start()
    {
        // get the transform of the main camera
        if (Camera.main != null)
            m_Cam = Camera.main.transform;
        else
        {
            Debug.LogWarning(
                "Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.", gameObject);
        }

        // get the third person character ( this should never be null due to require component )
        m_Character = GetComponent<ThirdPersonCharacter>();
    }

    private void Update()
    {
        if (!m_Jump)
            m_Jump = Input.GetButtonDown("Jump");

        // read inputs
        h = Input.GetAxisRaw("Horizontal");
        v = 1;//Input.GetAxisRaw("Vertical");

        // TARGETTING SYSTEM [WIP]
        // if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D)){
        //     if(targetAngle > 5)
        //         h = 1;
        //     if(targetAngle < -5)
        //         h = -1;
        //     if(targetAngle <= 5 && targetAngle >= -5)
        //         h = 0;
        // }
    }

    private void FixedUpdate()
    {
        Vector3 m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;

        // Get camera angle to selected target
        if(closestTarget != null){
            Vector3 targetDir = closestTarget.transform.position - transform.position;
            targetAngle = Vector3.SignedAngle(targetDir, transform.forward, Vector3.forward);
        }

        // calculate move direction to pass to character
        if (m_Cam != null){
            m_Move = (v * m_CamForward) + (h * m_Cam.right);
        }
        else{
            // we use world-relative directions in the case of no main camera
            m_Move = (v * Vector3.forward) + (h * Vector3.right);
        }

        // pass all parameters to the character control script
        m_Character.Move(m_Move, m_Jump);
        m_Jump = false;
    }
}
