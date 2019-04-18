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
    public float force,horizontalSpeed,verticalSpeed;
    
    
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
    }

    private void FixedUpdate()
    {
        // read inputs
        float h = Input.GetAxisRaw("Horizontal");
        float v = 1;//Input.GetAxisRaw("Vertical");
        Vector3 m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
        hover.z = horizontalSpeed;
        hover.x = Mathf.Sin(Time.realtimeSinceStartup*verticalSpeed) * force;
        //m_Move = new Vector3(h, 0, v);
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {

            m_Move = new Vector3(h, 0, 1);
        }
        else
        {
            m_Move = hover;
        }
        // calculate move direction to pass to character
        if (m_Cam != null)
        {
            // calculate camera relative direction to move:
            
            //m_Move = (v * m_CamForward) + (h * m_Cam.right);
        }
        else
        {
            // we use world-relative directions in the case of no main camera
            m_Move = v * Vector3.forward + h * Vector3.right;
        }

        // pass all parameters to the character control script
        m_Character.Move(m_Move, m_Jump);
        m_Jump = false;
    }
}
