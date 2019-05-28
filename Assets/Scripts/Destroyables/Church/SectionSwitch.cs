using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectionSwitch : MonoBehaviour
{
    public GameObject brokenSection;

    private TextFlash flash;

    private void Start() {
        flash = FindObjectOfType<TextFlash>();        
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
        foreach(Transform piece in brokenSection.transform)
            brokenSection.GetComponentInChildren<GravityTriggerController>().triggerChainReaction = true;
        Destroy(gameObject);
    }
}