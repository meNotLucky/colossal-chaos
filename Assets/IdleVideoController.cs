using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Video;

public class IdleVideoController : MonoBehaviour
{
    public GameObject idleVideo;
    public GameObject GUI;
    public float timeBeforeIdle;

    private float timer;

    void FixedUpdate()
    {
        if(Input.GetAxis("Mouse X") < 0 || Input.GetAxis("Mouse X") > 0){
            timer = 0;
            idleVideo.SetActive(false);
            GUI.SetActive(true);
        }

        if(timer < timeBeforeIdle)
            timer += Time.deltaTime;
        else {
            idleVideo.SetActive(true);
            GUI.SetActive(false);
        }
    }
}
