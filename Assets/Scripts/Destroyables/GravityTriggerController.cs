using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityTriggerController : MonoBehaviour
{
        List<GameObject> connectedPieces = new List<GameObject>();
        public bool collisionTriggered = false;
        bool componentsDeleted = true;
        int updateCount = 0;

    private void Start() {
        // Quick fix for Unity planar bug: [Physics.PhysX] QuickHullConvexHullLib::findSimplex: Simplex input points appers to be coplanar.
        Mesh mesh = GetComponent<MeshCollider>().sharedMesh;
        Vector3[] verts = mesh.vertices;
        Vector3 v = verts[0];
        verts[0] += Vector3.one * 0.00001f;
        mesh.vertices = verts;
        GetComponent<MeshCollider>().convex = true;
        verts[0] = v;
        mesh.vertices = verts;   
    }

    private void FixedUpdate() {

        if(collisionTriggered){
            if(updateCount < 5)
                updateCount++;
            else {
                if(!componentsDeleted){
                    if(GetComponent<Rigidbody>().IsSleeping()){

                        Destroy(GetComponent<Rigidbody>());
                        Destroy(GetComponent<MeshCollider>());
                        Destroy(GetComponent<GravityTriggerController>());
                        componentsDeleted = true;

                        //Destroy(gameObject); //<- Alternative performance saver
                    }
                }
            }
        }
    }

    private void OnCollisionEnter(Collision other) {

        if(other.gameObject.tag == "barrel"){
            if(!collisionTriggered){

                if(GetComponent<Rigidbody>().isKinematic == true)
                    GetComponent<Rigidbody>().isKinematic = false;
                    collisionTriggered = true;
            }
        }
        if(collisionTriggered){
            if(other.gameObject.tag == "floor")
                componentsDeleted = false;
        }
    }

}
