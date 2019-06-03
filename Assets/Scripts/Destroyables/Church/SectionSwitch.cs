using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectionSwitch : MonoBehaviour
{
    public GameObject brokenSection;

    private ScoreFeedback scoreFeed;
    private VoiceLineController voiceLine;
    private int loosePoints;

    private void Start() {
        scoreFeed = FindObjectOfType<ScoreFeedback>();
        voiceLine = FindObjectOfType<VoiceLineController>();
        loosePoints = GetComponentInParent<AttractionTarget>().loosePointsOnDestruction;
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
        if(scoreFeed != null)
            scoreFeed.InitializeFeedback(loosePoints);
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