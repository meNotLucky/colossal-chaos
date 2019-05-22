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

    private GiantUserInputV2 giantUserInputV2;
    private LooseCondition looseCondition;
    private BillboardFacing billboard;

    private void Start() {
        currentHitPoints = hitPoints;
        giantUserInputV2 = FindObjectOfType<GiantUserInputV2>();
        looseCondition = FindObjectOfType<LooseCondition>();
        billboard = GetComponentInChildren<BillboardFacing>();

        billboard.gameObject.SetActive(false);
    }

    private void Update() {

        if(currentHitPoints > 0){
            Collider[] hitObjects = Physics.OverlapSphere(transform.position, range, mask);
            if(hitObjects.Length > 0){
                foreach (var item in hitObjects){
                    if(item.gameObject.tag == "barrel"){
                        item.GetComponent<GiantUserInputV2>().SetTarget(gameObject);
                        billboard.gameObject.SetActive(true);
                    }
                }
            } else {
                billboard.gameObject.SetActive(false);
            }
        }
        else if(currentHitPoints <= 0){
            looseCondition.landmarkDestructionPoints += loosePointsOnDestruction;
            if(giantUserInputV2 != null)
                giantUserInputV2.RemoveCurrentTarget();
            billboard.gameObject.SetActive(false);
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
