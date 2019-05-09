using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttractionTarget : MonoBehaviour
{
    public float range;
    public int hitPoints;
    [HideInInspector] public int currentHitPoints;

    private void Start() {
        currentHitPoints = hitPoints;
    }

    private void Update() {
        if(currentHitPoints <= 0){
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
