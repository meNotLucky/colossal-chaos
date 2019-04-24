using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectionSwitch : MonoBehaviour
{
    public GameObject brokenSection;
    
    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.tag == "barrel"){
            brokenSection.SetActive(true);
            Destroy(gameObject);
        }
    }
}
