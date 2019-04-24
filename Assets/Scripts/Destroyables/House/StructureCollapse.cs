using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureCollapse : MonoBehaviour
{
    public List<GameObject> supportStructures = new List<GameObject>();
    private List<GameObject> destroyedSupportStructures = new List<GameObject>();
    public int mininumSupports;
    private int supportsDestroyed = 0;

    void FixedUpdate()
    {
        for (int i = 0; i < supportStructures.Count; i++)
        {
            Rigidbody[] rigidbodies = supportStructures[i].GetComponentsInChildren<Rigidbody>();
            foreach (var rigidbody in rigidbodies)
            {
                if(!destroyedSupportStructures.Contains(supportStructures[i])){
                    if(!rigidbody.isKinematic){
                        supportsDestroyed++;
                        destroyedSupportStructures.Add(supportStructures[i]);
                    }
                }
            }
        }

        if(supportsDestroyed >= mininumSupports){
            foreach(var controller in GetComponentsInChildren<GravityTriggerController>()){
                controller.triggerChainReaction = true;
            }
            
            Destroy(this);
        }
    }
}
