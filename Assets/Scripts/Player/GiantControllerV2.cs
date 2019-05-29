using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MyBox;

public class GiantControllerV2 : MonoBehaviour
{
    [Header("Giant Properties")]
    public float speed = 5f;
    public float speedMultiplier = 1f;
    public float turnSpeed = 10;
    public float height = 0.5f;
    public float heightPadding = 0.05f;
    public LayerMask ground;
    public float maxGroundAngle = 120;

    [Header("Random Speed Increase")]
    [Tooltip("Change the speed of the giant randomly")]
    public bool doRandomSpeed = false;
    [Tooltip("What range the speed can change between (If Move Speed Multipl. is 1.5 and Random Speed Offset is 0.5, min speed will be 1 and max speed will be 2)")]
    [ConditionalField("doRandomSpeed")] public float randomSpeedOffset = 0.5f;
	[Tooltip("The minimum amount of time between the random speed changes")]
    [ConditionalField("doRandomSpeed")] public float minTimeToSpeedChange = 0.5f;
	[Tooltip("The maximum amount of time between the random speed changes")]
    [ConditionalField("doRandomSpeed")] public float maxTimeToSpeedChange = 2.0f;

    [Header("Constant Speed Increase")]
    [Tooltip("Increase the speed of the giant constantly")]
    public bool doConstantSpeed = false;
    [Tooltip("How fast should the speed increase")]
    [ConditionalField("doConstantSpeed")] public float speedIncreaseMultiplier;
    [Tooltip("The max speed of the giant")]
    [ConditionalField("doConstantSpeed")] public float maxSpeedMultiplier;

    [Header("Stopping Properties")]
    public float stopSpeed = 0.2f;
	public float stopCooldown = 5.0f;
	public float stopDuration = 2.0f;
	private float stopCooldownTimer;
	private float stopDurationTimer;
	private bool playerStopped = false;
    private bool cannotSideStep;

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
    bool wallHit;
    float wallHitTimer;

    Animator animator;
    Rigidbody rigidbody;
    IEnumerator coroutine;

    bool startPanFinished = false;
    bool animationResetAfterPan = false;

    private void Start() {
        cam = Camera.main.transform;
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();
    }

    public void Move(float h, bool move, bool stop, bool sideStepLeft, bool sideStepRight) {
        
        input.x = h;
        input.y = move ? 1 : 0;

        if(doRandomSpeed)
            HandleRandomSpeed();
        if(doConstantSpeed)
            HandleConstantSpeed();

        HandleStopping(stop);
        HandleSideStep(sideStepLeft, sideStepRight);

        CalculateDirection();
        CalculateForward();
        CalculateGroundAngle();
        
        CheckGround();
        ApplyGravity();

        if(Mathf.Abs(input.x) < 1 && Mathf.Abs(input.y) < 1)
            return;

        if(wallHit){
            wallHitTimer = 0;
            rigidbody.isKinematic = false;
            rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        } else {
            if(wallHitTimer < 0.7f){
                wallHitTimer += Time.deltaTime;
            } else {
                wallHitTimer = 0.7f;
                rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
                rigidbody.isKinematic = true;
            }
        }
        
        if(startPanFinished){
            ApplyRotation();
            ApplyMovement();
        }

        if (animationResetAfterPan){
            animator.SetBool("Stopped", false);
            animationResetAfterPan = false;
        }

        UpdateAnimator();
    }

    public void HandleStartPan(bool isPlaying){
        if(isPlaying) {
            animator.SetBool("Stopped", true);
            forwardAmount = 0;
            startPanFinished = false;
            cannotSideStep = true;
        } else {
            animationResetAfterPan = true;
            startPanFinished = true;
        }
    }

    private void HandleRandomSpeed(){
        if(!animator.GetBool("Stopped") && !animator.GetBool("SideStepLeft") && !animator.GetBool("SideStepRight") && startPanFinished){
			float newSpeed = Random.Range(speedMultiplier - randomSpeedOffset, speedMultiplier + randomSpeedOffset);
			float timeToChange = Random.Range(minTimeToSpeedChange, maxTimeToSpeedChange);
			if(!newForwardGotten) {
				newForwardGotten = true;
				coroutine = MoveSpeedInterpolator(forwardAmount, newSpeed, timeToChange);
				StartCoroutine(coroutine);
			}
		}
    }

    private void HandleConstantSpeed(){
        if(!animator.GetBool("Stopped") && !animator.GetBool("SideStepLeft") && !animator.GetBool("SideStepRight") && startPanFinished){
            if(forwardAmount < maxSpeedMultiplier){
                if(forwardAmount == 0)
                    forwardAmount = 0.5f;
                forwardAmount += speedIncreaseMultiplier;
            } else {
                forwardAmount = maxSpeedMultiplier;
            }
		} else {
            // if(coroutine != null)
            //     StopCoroutine(coroutine);
        }
    }

    private void HandleStopping(bool stop)
	{
		// Check stop conditions
		if(stop && stopCooldownTimer <= 0){
            cannotSideStep = true;
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
                if(coroutine != null)
				    StopCoroutine(coroutine);
				StartCoroutine(MoveSpeedInterpolator(forwardAmount, stopSpeed, 1.0f));
			}
		} else if(stopDurationTimer < 0 && animator.GetBool("Stopped")){
			stopDurationTimer = 0;
			playerStopped = false;
			//StartCoroutine(MoveSpeedInterpolator(forwardAmount, 1, stopDuration / 2));
			animator.SetBool("Stopped", false);
		}
	}

    private void HandleSideStep(bool left, bool right)
	{
        if(grounded && groundAngle <= maxGroundAngle && !cannotSideStep && !wallHit){
            if(currentDeceleration > 0){
                delayTimer -= Time.deltaTime;
                if(coroutine != null)
                    StopCoroutine(coroutine);
                newForwardGotten = false;
                forwardAmount = sideStepForwardSpeed;

                Vector3 side = Vector3.Cross(transform.forward, hitInfo.normal);

                if(delayTimer <= 0){
                    currentDeceleration -= Time.deltaTime;
                    if(leftPressed){
                        transform.localPosition += side * sideStepPower * currentDeceleration;
                    }
                    else if(rightPressed){
                        transform.localPosition -= side * sideStepPower * currentDeceleration;
                    }
                    if(!animator.GetCurrentAnimatorStateInfo(0).IsName("Walking")){
                        animator.SetBool("SideStepLeft", false);
                        animator.SetBool("SideStepRight", false);
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
        else {
            delayTimer = 0;
            currentDeceleration = 0;
        }

        if(currentDeceleration <= 0 || wallHit){
            currentDeceleration = 0;
            delayTimer = 0;

            animator.SetBool("SideStepLeft", false);
            animator.SetBool("SideStepRight", false);

            leftPressed = false;
            rightPressed = false;
        }
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

        if(!wallHit){
            transform.position += forward * speed * forwardAmount * Time.deltaTime;
        }
        else if(wallHit){
            rigidbody.velocity = forward * (speed * 10.0f) * forwardAmount * Time.deltaTime;
        }
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
            if(Vector3.Distance(transform.position, hitInfo.point) < height)
                transform.position = Vector3.Lerp(transform.position, transform.position + Vector3.up * height, 5 * Time.deltaTime);
            grounded = true;
        } else {
            grounded = false;
        }
    }

    private void ApplyGravity () {
        if(!grounded && !leftPressed && !rightPressed && !wallHit){
            transform.position += Physics.gravity * Time.deltaTime;
        }
    }

    private void UpdateAnimator()
	{
		// update the animator parameters
		animator.SetFloat("Forward", forwardAmount, 0.1f, Time.deltaTime);
		animator.SetBool("OnGround", grounded);

		if(!animator.GetBool("SideStepLeft") && !animator.GetBool("SideStepRight") && !animator.GetCurrentAnimatorStateInfo(0).IsName("Idle")){
            animator.speed = forwardAmount;
            if(!animator.GetBool("Stopped"))
                cannotSideStep = false;
        }
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

    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.tag == "Wall")
            wallHit = true;
    }

    private void OnCollisionExit(Collision other) {
        if(other.gameObject.tag == "Wall")
            wallHit = false;
    }
}
