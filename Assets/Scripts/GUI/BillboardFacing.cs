using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;
using MyBox;

public class BillboardFacing : MonoBehaviour
{
    [Header("Properties")]
    public bool animateSize;
    [ConditionalField("animateSize")] public float sizeAnimationSpeed = 3.0f;
    [ConditionalField("animateSize")] public float sizeModifier = 0.6f;

    public bool animateUpDown;
    [ConditionalField("animateUpDown")] public float upDownAnimationSpeed = 3.0f;
    [ConditionalField("animateUpDown")] public float distanceModifier = 0.6f;
    void Update()
    {
        transform.forward = Camera.main.transform.forward;
        GetComponent<SpriteRenderer>().enabled = false;

        if(animateSize){
            float val = Mathf.Sin(Time.realtimeSinceStartup * sizeAnimationSpeed);
            if(val < 0){ val *= -1; }
            val += sizeModifier;
            transform.localScale = new Vector3(val, val, val);
        }

        if(animateUpDown){
            float val = Mathf.Sin(Time.realtimeSinceStartup * upDownAnimationSpeed);
            if(val < 0){ val *= -1; }
            val += distanceModifier;
            transform.localPosition = new Vector3(transform.localPosition.x, val, transform.localPosition.z);
        }
    }
}
