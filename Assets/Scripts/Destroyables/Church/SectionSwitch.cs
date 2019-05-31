using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectionSwitch : MonoBehaviour
{
    public GameObject brokenSection;

    private TextFlash flash;
    private VoiceLineController voiceLine;

    private void Start() {
        flash = FindObjectOfType<TextFlash>();
        voiceLine = FindObjectOfType<VoiceLineController>();     
    }
    
    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.tag == "barrel"){
            SwitchSection();
        }
    }
    
    public void SwitchSection() {
        brokenSection.SetActive(true);
        if(GetComponentInParent<AttractionTarget>() != null)
            GetComponentInParent<AttractionTarget>().currentHitPoints--;
        if(flash != null)
            flash.Flash();
        if(voiceLine != null){
            if(GetComponent<MeshRenderer>() !=  null){
                foreach (var material in GetComponent<MeshRenderer>().materials){
                    if(material.name == "Church_Spire_M"){
                        voiceLine.PlayRandomSetLine("ChurchBreaking");
                    }
                }
            }
            voiceLine.PlayRandomSetLine("HouseBreaking");
        }
        foreach(Transform piece in brokenSection.transform)
            brokenSection.GetComponentInChildren<GravityTriggerController>().triggerChainReaction = true;
        Destroy(gameObject);
    }
}