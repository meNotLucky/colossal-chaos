using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakSwitch : MonoBehaviour
{
    public GameObject brokenHouse;
    
    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.tag == "barrel"){
            Instantiate(brokenHouse, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
