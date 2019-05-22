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

    public bool animateUpDown;
    [ConditionalField("animateUpDown")] public float upDownAnimationSpeed = 3.0f;

    private float scale;
    private float posY;

    void Start()
    {
        scale = transform.localScale.x;
        posY = transform.localPosition.y;
    }

    void Update()
    {
        //transform.rotation = new Quaternion(transform.rotation.x, Camera.main.transform.rotation.y, transform.rotation.z, transform.rotation.w);
        transform.LookAt(Camera.main.transform, Vector3.up);

        if(animateSize){
            float val = Mathf.Sin(Time.realtimeSinceStartup * sizeAnimationSpeed);
            if(val < 0){ val *= -1; }
            transform.localScale = new Vector3(scale + val, scale + val, scale + val);
        }

        if(animateUpDown){
            float val = Mathf.Sin(Time.realtimeSinceStartup * upDownAnimationSpeed);
            transform.localPosition = new Vector3(transform.localPosition.x, posY + val, transform.localPosition.z);
        }
    }
}
