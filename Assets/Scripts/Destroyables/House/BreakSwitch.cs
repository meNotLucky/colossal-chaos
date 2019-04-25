using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakSwitch : MonoBehaviour
{
    public GameObject brokenHouse;
    public GameObject[] dustPiles;
    
    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.tag == "barrel"){
            Instantiate(brokenHouse, transform.position, transform.rotation);

            int index = Random.Range(0, dustPiles.Length);
            Instantiate(dustPiles[index], transform.position, transform.rotation);

            Destroy(gameObject);
        }
    }
}
