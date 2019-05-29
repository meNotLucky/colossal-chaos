using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollController : MonoBehaviour
{
    public Animator giantAnimator;
    public Rigidbody giantRigidbody;
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
        giantAnimator.enabled = false;
        giantRigidbody.isKinematic = false;
        giantRigidbody.useGravity = true;

        foreach (var body in rigidbodies)
        {
            body.isKinematic = false;
            body.useGravity = false;
        }
    }
}
