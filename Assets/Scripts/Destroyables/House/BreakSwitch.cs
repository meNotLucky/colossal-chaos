using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakSwitch : MonoBehaviour
{
    public GameObject brokenMesh;
    public GameObject[] dustPiles;
    
    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.tag == "barrel"){
            Switch();
        }
    }

    public void Switch(){
        Instantiate(brokenMesh, transform.position, transform.rotation);

        if(dustPiles.Length > 0){
            int index = Random.Range(0, dustPiles.Length);
            Instantiate(dustPiles[index], transform.position, transform.rotation);
        }

        Destroy(gameObject);
    }
}
