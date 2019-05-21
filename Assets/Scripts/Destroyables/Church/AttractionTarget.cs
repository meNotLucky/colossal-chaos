using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttractionTarget : MonoBehaviour
{
    [Header("Target Properties")]
    public float range;
    public int hitPoints;
    public LayerMask mask = 8;

    [Header("Gameplay Properties")]
    public int loosePointsOnDestruction;

    [HideInInspector] public int currentHitPoints;

    private void Start() {
        currentHitPoints = hitPoints;
    }

    private void Update() {

        if(currentHitPoints > 0){
            Collider[] hitObjects = Physics.OverlapSphere(transform.position, range, mask);
            if(hitObjects.Length > 0){
                foreach (var item in hitObjects){
                    if(item.gameObject.tag == "barrel"){
                        item.GetComponent<GiantUserInputV2>().SetTarget(gameObject);
                    }
                }
            }
        }
        else if(currentHitPoints <= 0){
            FindObjectOfType<LooseCondition>().landmarkDestructionPoints += loosePointsOnDestruction;
            if(FindObjectOfType<GiantUserInputV2>() != null)
                FindObjectOfType<GiantUserInputV2>().RemoveCurrentTarget();
            else if(FindObjectOfType<GiantUserInput>() != null)
                FindObjectOfType<GiantUserInput>().RemoveCurrentTarget();
            Destroy(this);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    public float GetRange()
    {
        return range;
    }
}
