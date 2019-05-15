using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectionSwitch : MonoBehaviour
{
    public GameObject brokenSection;
    
    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.tag == "barrel"){
            SwitchSection();
        }
    }
    
    public void SwitchSection() {
        brokenSection.SetActive(true);
        if(GetComponentInParent<AttractionTarget>() != null)
            GetComponentInParent<AttractionTarget>().currentHitPoints--;
        foreach(Transform piece in brokenSection.transform)
            brokenSection.GetComponentInChildren<GravityTriggerController>().triggerChainReaction = true;
        Destroy(gameObject);
    }
}