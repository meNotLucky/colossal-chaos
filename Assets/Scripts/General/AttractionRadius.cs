using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttractionRadius : MonoBehaviour
{    public float maxRadius;

   private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, maxRadius);
    }

    public float GetRange()
    {
        return maxRadius;
    }
}
