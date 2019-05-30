using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollController : MonoBehaviour
{
    public Animator giantAnimator;
    public Rigidbody giantRigidbody;
    public GiantControllerV2 giantController;
    public GiantUserInputV2 giantInput;
    Rigidbody[] rigidbodies;

    private void Start() {
        rigidbodies = GetComponentsInChildren<Rigidbody>();
        foreach (var body in rigidbodies)
        {
            body.isKinematic = true;
            body.useGravity = false;
        }
    }

    public void ActivateRagdoll()
    {
        giantRigidbody.isKinematic = false;
        giantRigidbody.useGravity = true;
        giantAnimator.enabled = false;
        //giantController.enabled = false;
        giantInput.rotate = false;

        foreach (var body in rigidbodies)
        {
            body.isKinematic = false;
            body.useGravity = true;
        }
    }
}
