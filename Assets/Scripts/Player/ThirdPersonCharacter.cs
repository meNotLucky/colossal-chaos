using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class ThirdPersonCharacter : MonoBehaviour
{
	[Header("Giant Properties")]
	[SerializeField] float m_MoveSpeedMultiplier = 1f;
	[SerializeField] float m_RandomSpeedOffset = 0.5f;
	[SerializeField] float m_MinTimeToSpeedChange = 0.5f;
	[SerializeField] float m_MaxTimeToSpeedChange = 2.0f;
	[SerializeField] float m_MovingTurnSpeed = 360;
	[SerializeField] float m_StationaryTurnSpeed = 180;
	[SerializeField] float m_JumpPower = 12f;
	[Range(1f, 4f)][SerializeField] float m_GravityMultiplier = 2f;
	[SerializeField] float m_RunCycleLegOffset = 0.2f; //specific to the character in sample assets, will need to be modified to work with others
	[SerializeField] float m_GroundCheckDistance = 0.1f;

	[Header("Stopping Properties")]
	[SerializeField] float stopCooldown = 5.0f;
	[SerializeField] float stopDuration = 2.0f;
	private float stopCooldownTimer;
	private float stopDurationTimer;
	private bool playerStopped = false;

	[Header("Side Step Properties")]
	[SerializeField] float sideStepCooldown = 2.0f;
	[SerializeField] float sideStepPower = 50.0f;
	[SerializeField] float sideStepDeceleration = 0.7f;
	[SerializeField] float delayForAnimation = 0.2f;
	[SerializeField][Range(0.0f, 1.0f)] float sideStepForwardSpeed = 0.7f;
	private float sideStepCooldownTimer;
	private float currentDeceleration;
	private bool leftPressed, rightPressed;
	private float delayTimer;
	
	Rigidbody m_Rigidbody;
	Animator m_Animator;
	bool m_IsGrounded;
	float m_OrigGroundCheckDistance;
	const float k_Half = 0.5f;
	float m_TurnAmount;
	float m_ForwardAmount;
	bool m_NewForwardGotten;
	Vector3 m_GroundNormal;


	void Start()
	{
		m_Animator = GetComponent<Animator>();
		m_Rigidbody = GetComponent<Rigidbody>();

		m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
		m_OrigGroundCheckDistance = m_GroundCheckDistance;
	}

	public void Move(Vector3 move, bool jump, bool stop, bool sideStepLeft, bool sideStepRight)
	{
		// convert the world relative moveInput vector into a local-relative
		// turn amount and forward amount required to head in the desired
		// direction.
		if (move.magnitude > 1f) move.Normalize();
		move = transform.InverseTransformDirection(move);
		CheckGroundStatus();
		move = Vector3.ProjectOnPlane(move, m_GroundNormal);
		m_TurnAmount = Mathf.Atan2(move.x, move.z);

		ApplyExtraTurnRotation();

		// control and velocity handling is different when grounded and airborne:
		if (m_IsGrounded){
			HandleGroundedMovement(move,jump);
			HandleStopping(stop);
			HandleSideStep(sideStepLeft, sideStepRight);
		}
		else {
			HandleAirborneMovement();
		}

		// send input and other state parameters to the animator
		UpdateAnimator(move);
	}

	void UpdateAnimator(Vector3 move)
	{
		// update the animator parameters
		m_Animator.SetFloat("Forward", m_ForwardAmount, 0.1f, Time.deltaTime);
		m_Animator.SetBool("OnGround", m_IsGrounded);
		
		if (!m_IsGrounded)
		{
			m_Animator.SetFloat("Jump", m_Rigidbody.velocity.y);
		}

		if(!m_Animator.GetBool("Stopped") && !m_Animator.GetBool("SideStepLeft") && !m_Animator.GetBool("SideStepRight"))
			m_Animator.speed = m_ForwardAmount;
		else
			m_Animator.speed = 1;
	}

	void HandleAirborneMovement()
	{
		// apply extra gravity from multiplier:
		Vector3 extraGravityForce = (Physics.gravity * m_GravityMultiplier) - Physics.gravity;
		m_Rigidbody.AddForce(extraGravityForce);

		m_GroundCheckDistance = m_Rigidbody.velocity.y < 0 ? m_OrigGroundCheckDistance : 0.01f;
	}

	void HandleStopping(bool stop)
	{
		// Check stop conditions
		if(stop && stopCooldownTimer <= 0){
			stopCooldownTimer = stopCooldown;
			stopDurationTimer = stopDuration;

			m_Animator.SetBool("Stopped", true);
		}

		if(stopCooldownTimer > 0)
			stopCooldownTimer -= Time.deltaTime;
		else
			stopCooldownTimer = 0;

		if(stopDurationTimer > 0){
			stopDurationTimer -= Time.deltaTime;
			if(!playerStopped){
				playerStopped = true;
				StartCoroutine(MoveSpeedInterpolator(m_ForwardAmount, 0, stopDuration / 2));
			}
		} else if(stopDurationTimer < 0 && m_Animator.GetBool("Stopped")) {
			stopDurationTimer = 0;
			playerStopped = false;
			StartCoroutine(MoveSpeedInterpolator(m_ForwardAmount, 1, stopDuration / 2));
			m_Animator.SetBool("Stopped", false);
		}
	}

	void HandleSideStep(bool left, bool right)
	{
		if(currentDeceleration > 0){
			delayTimer -= Time.deltaTime;
			m_ForwardAmount = sideStepForwardSpeed;

			if(delayTimer <= 0){
				currentDeceleration -= Time.deltaTime;
				if(leftPressed)
					m_Rigidbody.AddRelativeForce(Vector3.left * sideStepPower * currentDeceleration, ForceMode.VelocityChange);
				else if(rightPressed)
					m_Rigidbody.AddRelativeForce(Vector3.right * sideStepPower * currentDeceleration, ForceMode.VelocityChange);

				if(!m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Walking")){
					m_Animator.SetBool("SideStepLeft", false);
					m_Animator.SetBool("SideStepRight", false);
				}

				if(currentDeceleration <= 0){
					currentDeceleration = 0;
					delayTimer = 0;

					leftPressed = false;
					rightPressed = false;

					m_ForwardAmount = 1;
				}
			}
		}

		if((left || right) && sideStepCooldownTimer <= 0){
			sideStepCooldownTimer = sideStepCooldown;
			currentDeceleration = sideStepDeceleration;
			delayTimer = delayForAnimation;

			if(right){
				rightPressed = true;
				m_Animator.SetBool("SideStepRight", true);
			}
			else if(left){
				leftPressed = true;
				m_Animator.SetBool("SideStepLeft", true);
			}
		}

		if(sideStepCooldownTimer > 0)
			sideStepCooldownTimer -= Time.deltaTime;
		else
			sideStepCooldownTimer = 0;
	}

	void HandleGroundedMovement(Vector3 move, bool jump)
	{
		// Check jump conditions
		if (jump && m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Grounded"))
		{
			m_Rigidbody.velocity = new Vector3(m_Rigidbody.velocity.x, m_JumpPower, m_Rigidbody.velocity.z);

			m_IsGrounded = false;
			m_Animator.applyRootMotion = false;
			m_GroundCheckDistance = 0.1f;
		}

		// Get and apply random speed
		if(!m_Animator.GetBool("Stopped") && !m_Animator.GetBool("SideStepLeft") && !m_Animator.GetBool("SideStepRight")){
			float newSpeed = Random.Range(m_MoveSpeedMultiplier - m_RandomSpeedOffset, m_MoveSpeedMultiplier + m_RandomSpeedOffset);
			float timeToChange = Random.Range(m_MinTimeToSpeedChange, m_MaxTimeToSpeedChange);
			if(!m_NewForwardGotten){
				m_NewForwardGotten = true;
				StartCoroutine(MoveSpeedInterpolator(m_ForwardAmount, newSpeed, timeToChange));
			}
		}

		Vector3 localVelocity = transform.InverseTransformDirection(m_Rigidbody.velocity);
		localVelocity = ((move / 10) * m_MoveSpeedMultiplier * m_ForwardAmount) / Time.deltaTime;
		m_Rigidbody.velocity = transform.TransformDirection(localVelocity);
	}

	void ApplyExtraTurnRotation()
	{
		// help the character turn faster (this is in addition to root rotation in the animation)
		float turnSpeed = Mathf.Lerp(m_StationaryTurnSpeed, m_MovingTurnSpeed, m_ForwardAmount);
		transform.Rotate(0, m_TurnAmount * turnSpeed * Time.deltaTime, 0);
	}

	void CheckGroundStatus()
	{
		RaycastHit hitInfo;

		if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hitInfo, m_GroundCheckDistance))
		{
			m_GroundNormal = hitInfo.normal;
			m_IsGrounded = true;
			m_Animator.applyRootMotion = true;
		}
		else
		{
			m_IsGrounded = false;
			m_GroundNormal = Vector3.up;
			m_Animator.applyRootMotion = false;
		}
	}

	IEnumerator MoveSpeedInterpolator(float startValue, float endValue, float duration)
    {
        float elapsed = 0.0f;
        while (elapsed < duration)
        {
            m_ForwardAmount = Mathf.Lerp(startValue, endValue, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        m_ForwardAmount = endValue;
		m_NewForwardGotten = false;
    }
}
