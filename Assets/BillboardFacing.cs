using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardFacing : MonoBehaviour
{
    public float animationSpeed = 3.0f;
    public float sizeModifier = 0.6f;

    void Update()
    {
        transform.forward = Camera.main.transform.forward;
        GetComponent<SpriteRenderer>().enabled = false;

        float val = Mathf.Sin(Time.realtimeSinceStartup * animationSpeed);
        if(val < 0){ val *= -1; }
        val += sizeModifier;
        transform.localScale = new Vector3(val, val, val);

        if(GetComponentInParent<AttractionTarget>() != null){
            Collider[] hitObjects = Physics.OverlapSphere(transform.position, GetComponentInParent<AttractionTarget>().range);
            foreach (var item in hitObjects)
            {
                if(item.gameObject.tag == "barrel"){
                    GetComponent<SpriteRenderer>().enabled = true;
                }
            }
        }
    }
}
