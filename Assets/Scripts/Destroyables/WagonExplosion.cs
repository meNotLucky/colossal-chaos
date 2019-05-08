using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WagonExplosion : MonoBehaviour
{
    [Header("Properties")]
    public float range;
    public float explosionForce;

    Animator anim;
    [SerializeField] bool exploading = false;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update() {
        if(exploading){
            Collider[] hitObjects = Physics.OverlapSphere(transform.position, range);
            if(hitObjects.Length > 0){
                Explode();
            } else {
                EndExplosion();
            }
        }
    }

    private void Explode(){
        
        exploading = true;

        Collider[] hitObjects = Physics.OverlapSphere(transform.position, range);
        foreach (var item in hitObjects)
        {
            if(item.gameObject.tag == "Villager"){
                item.GetComponent<VillagerAI>().Die();
            }
            if(item.gameObject.tag == "house"){
                item.GetComponent<BreakSwitch>().Switch();
            }
            if(item.gameObject.tag == "house_piece"){
                if(item.GetComponent<GravityTriggerController>() != null)
                    item.GetComponent<GravityTriggerController>().EnableGravity();
                if(item.GetComponent<Rigidbody>() != null){
                    item.GetComponent<Rigidbody>().AddExplosionForce(explosionForce, transform.position, range);
                }
                EndExplosion();
            }
        }
    }

    private void EndExplosion(){
        exploading = false;

        if(GetComponent<BoxCollider>() != null)
            Destroy(GetComponent<BoxCollider>());

        if(GetComponent<LODGroup>() != null)
            Destroy(GetComponent<LODGroup>());

        Destroy(this);
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.tag == "barrel"){
            anim.SetBool("play", true);
            Explode();
        }
    }
}
