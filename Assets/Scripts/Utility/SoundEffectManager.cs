using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectManager : MonoBehaviour
{
    public List<AudioClip> clips;

    AudioSource audioSrc;
    
    void Start()
    {
        audioSrc=GetComponent<AudioSource>();
    }
    public void PlaySound(string clipToPlay)
    {
        foreach(AudioClip clip in clips)
        {
            
            if(clip.name == clipToPlay)
            audioSrc.PlayOneShot(clip);  
            
        }
    }
}
