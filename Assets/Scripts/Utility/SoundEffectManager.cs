using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectManager : MonoBehaviour
{
    public AudioClip rightSideStep;
    public AudioClip leftSideStep;
    public AudioClip Steps;

    AudioSource audioSrc;
    
    void Start()
    {
        audioSrc=GetComponent<AudioSource>();
    }
    public void PlaySound(int clipToPlay)
    {
            if(clipToPlay == 1)
            {
            audioSrc.PlayOneShot(leftSideStep);  
            audioSrc.panStereo =- 1;
            }
            else if(clipToPlay == 2)
            {
            audioSrc.PlayOneShot(rightSideStep);  
            audioSrc.panStereo =- 1;
            }
            else if(clipToPlay == 3)
            {
            audioSrc.PlayOneShot(Steps);
            }
               
    }
}
