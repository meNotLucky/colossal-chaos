using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class VoiceLineController : MonoBehaviour
{
    [System.Serializable]
    public class VoiceLineSet {
        public string name;
        public AudioClip[] clips;

        public AudioClip GetClip(int index){
            return clips[index];
        }

        public AudioClip GetRandomClip(){
            int index = Random.Range(0, clips.Length - 1);
            return clips[index];
        }
    }

    AudioSource source;
    public VoiceLineSet[] voiceLineSets;

    private void Start(){
        source = GetComponent<AudioSource>();
    }

    public void PlayRandomSetLine(string set){
        foreach (var lineSet in voiceLineSets)
            if(lineSet.name == set){
                int chanceToPlay = Random.Range(1, 5);
                if(chanceToPlay == 1){
                    source.PlayOneShot(lineSet.GetRandomClip());
                }
            }
    }
}
