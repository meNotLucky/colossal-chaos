using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinCondition : MonoBehaviour
{
    private void OnCollisionEnter(Collision other){
        if(other.gameObject.tag == "barrel")  
            Debug.Log("Win");
    }
}
