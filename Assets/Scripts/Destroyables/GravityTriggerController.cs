using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityTriggerController : MonoBehaviour
{
        List<GameObject> connectedPieces = new List<GameObject>();
        [SerializeField] bool collisionTriggered = false;
        public bool triggerChainReaction = false;
        bool componentsDeleted = true;
        [SerializeField] bool hasConnection = false;
        int updateCount = 0;
        float timeToDestroy;
        float timer;

    private void Start() {

        // Quick fix for Unity planar bug: [Physics.PhysX] QuickHullConvexHullLib::findSimplex: Simplex input points appers to be coplanar.
        // Mesh mesh = GetComponent<MeshCollider>().sharedMesh;
        // Vector3[] verts = mesh.vertices;
        // Vector3 v = verts[0];
        // verts[0] += Vector3.one * 0.00001f;
        // mesh.vertices = verts;
        // GetComponent<MeshCollider>().convex = true;
        // verts[0] = v;
        // mesh.vertices = verts;

        timeToDestroy = GlobalSettings.debrieLifeDuration;
    }

    private void FixedUpdate() {

        if(triggerChainReaction && !hasConnection)
            EnableGravity();
        else if(triggerChainReaction && hasConnection && RigidbodyCheck()){
            if(GetComponent<Rigidbody>().isKinematic == true){
                timer += Time.deltaTime;
                if(timer >= 1){
                    EnableGravity();
                    timer = 0;
                }
            }
        }

        if(collisionTriggered){
            if(componentsDeleted){
                timer += Time.deltaTime;
                if(timer >= timeToDestroy)
                    Destroy(gameObject);
            }
            if(updateCount < 5)
                updateCount++;
            else {
                if(!componentsDeleted){
                    if(RigidbodyCheck()){
                        if(GetComponent<Rigidbody>().IsSleeping()){
                            Destroy(GetComponent<Rigidbody>());
                            Destroy(GetComponent<MeshCollider>());
                            componentsDeleted = true;
                        }
                    }
                }
            }
        }
    }

    private void OnCollisionEnter(Collision other) {

        if(other.gameObject.tag == "barrel"){
            if(!collisionTriggered)
                EnableGravity();
        }
        if(collisionTriggered){
            if(other.gameObject.tag == "floor")
                componentsDeleted = false;
        }
    }

    private void OnCollisionExit(Collision other) {

        if(triggerChainReaction){
            EnableGravity();
            if(other.gameObject.GetComponent<GravityTriggerController>() != null)
                other.gameObject.GetComponent<GravityTriggerController>().triggerChainReaction = true;
        }
    }

    public void EnableGravity(){
        if(RigidbodyCheck()){
            if(GetComponent<Rigidbody>().isKinematic == true)
                GetComponent<Rigidbody>().isKinematic = false;
            collisionTriggered = true;
        }
    }

    private bool RigidbodyCheck(){
        return GetComponent<Rigidbody>() != null ? true : false;
    }
}