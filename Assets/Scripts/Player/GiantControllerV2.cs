using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantControllerV2 : MonoBehaviour
{
    [Header("Giant Properties")]
    public float speed = 5f;
    public float speedMultiplier = 1f;
    [Tooltip("What range the speed can change between (If Move Speed Multipl. is 1.5 and Random Speed Offset is 0.5, min speed will be 1 and max speed will be 2)")]
    public float randomSpeedOffset = 0.5f;
	[Tooltip("The minimum amount of time between the random speed changes")]
	public float minTimeToSpeedChange = 0.5f;
	[Tooltip("The maximum amount of time between the random speed changes")]
    public float maxTimeToSpeedChange = 2.0f;
    public float turnSpeed = 10;
    public float height = 0.5f;
    public float heightPadding = 0.05f;
    public LayerMask ground;
    public float maxGroundAngle = 120;

    [Header("Stopping Properties")]
	public float stopCooldown = 5.0f;
	public float stopDuration = 2.0f;
	private float stopCooldownTimer;
	private float stopDurationTimer;
	private bool playerStopped = false;

    [Header("Side Step Properties")]
	public float sideStepCooldown = 2.0f;
	public float sideStepPower = 50.0f;
	public float sideStepDeceleration = 0.7f;
	public float delayForAnimation = 0.2f;
	[Range(0.0f, 1.0f)] public float sideStepForwardSpeed = 0.7f;
	private float sideStepCooldownTimer;
	private float currentDeceleration;
	private bool leftPressed, rightPressed;
	private float delayTimer;

    Vector2 input;
    float forwardAmount = 1;
    bool newForwardGotten;
    float angle;
    float groundAngle;

    Quaternion targetRotation;
    Transform cam;

    Vector3 forward;
    RaycastHit hitInfo;
    bool grounded;

    Animator animator;
    IEnumerator coroutine;

    private void Start() {
        cam = Camera.main.transform;
        animator = GetComponent<Animator>();
    }

    public void Move(float h, bool move, bool stop, bool sideStepLeft, bool sideStepRight) {
        input.x = h;
        input.y = move ? 1 : 0;

        CalculateDirection();
        CalculateForward();
        CalculateGroundAngle();
        CheckGround();
        ApplyGravity();

        if(Mathf.Abs(input.x) < 1 && Mathf.Abs(input.y) < 1)
            return;
        
        HandleRandomSpeed();
        HandleStopping(stop);

        ApplyRotation();
        ApplyMovement();

        UpdateAnimator();
    }

    void HandleRandomSpeed(){
        if(!animator.GetBool("Stopped") && !animator.GetBool("SideStepLeft") && !animator.GetBool("SideStepRight")){
			float newSpeed = Random.Range(speedMultiplier - randomSpeedOffset, speedMultiplier + randomSpeedOffset);
			float timeToChange = Random.Range(minTimeToSpeedChange, maxTimeToSpeedChange);
			if(!newForwardGotten){
				newForwardGotten = true;
				coroutine = MoveSpeedInterpolator(forwardAmount, newSpeed, timeToChange);
				StartCoroutine(coroutine);
			}
		}
    }

    void HandleStopping(bool stop)
	{
		// Check stop conditions
		if(stop && stopCooldownTimer <= 0){
			stopCooldownTimer = stopCooldown;
			stopDurationTimer = stopDuration;

			animator.SetBool("Stopped", true);
		}

		if(stopCooldownTimer > 0)
			stopCooldownTimer -= Time.deltaTime;
		else
			stopCooldownTimer = 0;

		if(stopDurationTimer > 0){
			stopDurationTimer -= Time.deltaTime;
			if(!playerStopped){
				playerStopped = true;
				StopCoroutine(coroutine);
				StartCoroutine(MoveSpeedInterpolator(forwardAmount, 0, stopDuration / 2));
			}
		} else if(stopDurationTimer < 0 && animator.GetBool("Stopped")) {
			stopDurationTimer = 0;
			playerStopped = false;
			StartCoroutine(MoveSpeedInterpolator(forwardAmount, 1, stopDuration / 2));
			animator.SetBool("Stopped", false);
		}
	}

    void HandleSideStep(bool left, bool right)
	{
		if(currentDeceleration > 0){
			delayTimer -= Time.deltaTime;
			StopCoroutine(coroutine);
			newForwardGotten = false;
			forwardAmount = sideStepForwardSpeed;

			if(delayTimer <= 0){
				currentDeceleration -= Time.deltaTime;
				if(leftPressed){
					//m_Rigidbody.AddRelativeForce(Vector3.left * sideStepPower * currentDeceleration, ForceMode.VelocityChange);
                    //transform.localPosition = Vector3.left * sideStepPower * currentDeceleration;
                }
				else if(rightPressed){
					//m_Rigidbody.AddRelativeForce(Vector3.right * sideStepPower * currentDeceleration, ForceMode.VelocityChange);

                }

				if(!animator.GetCurrentAnimatorStateInfo(0).IsName("Walking")){
					animator.SetBool("SideStepLeft", false);
					animator.SetBool("SideStepRight", false);
				}

				if(currentDeceleration <= 0){
					currentDeceleration = 0;
					delayTimer = 0;

					leftPressed = false;
					rightPressed = false;

					forwardAmount = 1;
				}
			}
		}

		if((left || right) && sideStepCooldownTimer <= 0){
			sideStepCooldownTimer = sideStepCooldown;
			currentDeceleration = sideStepDeceleration;
			delayTimer = delayForAnimation;

			if(right){
				rightPressed = true;
				animator.SetBool("SideStepRight", true);
			}
			else if(left){
				leftPressed = true;
				animator.SetBool("SideStepLeft", true);
			}
		}

		if(sideStepCooldownTimer > 0)
			sideStepCooldownTimer -= Time.deltaTime;
		else
			sideStepCooldownTimer = 0;
	}

    private void CalculateDirection() {
        angle = Mathf.Atan2(input.x, input.y);
        angle = Mathf.Rad2Deg * angle;
        angle += cam.eulerAngles.y;
    }

    private void ApplyRotation() {
        targetRotation = Quaternion.Euler(0, angle, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
    }

    private void ApplyMovement() {
        if(groundAngle >= maxGroundAngle)
            return;
        transform.position += forward * speed * forwardAmount * Time.deltaTime;
    }

    private void CalculateForward () {
        if(!grounded){
            forward = transform.forward;
            return;
        }

        forward = Vector3.Cross(transform.right, hitInfo.normal);
    }

    private void CalculateGroundAngle () {
        if(!grounded){
            groundAngle = 90;
            return;
        }

        groundAngle = Vector3.Angle(hitInfo.normal, transform.forward);
    }

    private void CheckGround () {
        if(Physics.Raycast(transform.position, -Vector3.up, out hitInfo, height + heightPadding, ground)){
            if(Vector3.Distance(transform.position, hitInfo.point) < height){
                transform.position = Vector3.Lerp(transform.position, transform.position + Vector3.up * height, 5 * Time.deltaTime);
            }
            grounded = true;
        } else {
            grounded = false;
        }
    }

    private void ApplyGravity () {
        if(!grounded){
            transform.position += Physics.gravity * Time.deltaTime;
        }
    }

    void UpdateAnimator()
	{
		// update the animator parameters
		animator.SetFloat("Forward", forwardAmount, 0.1f, Time.deltaTime);
		animator.SetBool("OnGround", grounded);

		if(!animator.GetBool("Stopped") && !animator.GetBool("SideStepLeft") && !animator.GetBool("SideStepRight") && !animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
			animator.speed = forwardAmount;
		else
			animator.speed = 1;
	}

    IEnumerator MoveSpeedInterpolator(float startValue, float endValue, float duration)
    {
        float elapsed = 0.0f;
        while (elapsed < duration)
        {
            forwardAmount = Mathf.Lerp(startValue, endValue, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        forwardAmount = endValue;
		newForwardGotten = false;
    }
}
