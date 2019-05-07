using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class VillagerAI : MonoBehaviour
{
    NavMeshAgent navMeshAgent;

    [Header("AI Properties")]
    public float timeForNewPath;

    [Header("References")]
    public GameObject mesh;
    public GameObject bloodEffect;

    private bool inCoroutine;
    Vector3 target;
    NavMeshPath path;
    bool validPath;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        path = new NavMeshPath();
    }

    Vector3 getNewPos()
    {
        float x = Random.Range(285, 500);
        float z = Random.Range(450, 710);
        Vector3 pos = new Vector3(x, transform.position.y, z);
        return pos;
    }

    IEnumerator WaitForNextPath()
    {
        inCoroutine = true;
        yield return new WaitForSeconds(timeForNewPath);
        
        GetNewPath();
        validPath = navMeshAgent.CalculatePath(target,path);

        while(!validPath)
        {
            yield return new WaitForSeconds(0.01f);
            GetNewPath();
            validPath = navMeshAgent.CalculatePath(target, path);
        }

        inCoroutine=false;
    }

    void GetNewPath()
    {
        target = getNewPos();
        navMeshAgent.SetDestination(target);
    }

    void Update()
    {
        if(mesh.activeSelf){
            if(!inCoroutine)
                StartCoroutine(WaitForNextPath());
        }
        
        if(bloodEffect.activeSelf && bloodEffect.GetComponent<ParticleSystem>().isStopped){
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.tag == "barrel"){
            bloodEffect.SetActive(true);
            bloodEffect.GetComponent<ParticleSystem>().Play();
            mesh.SetActive(false);
        }
    }
}
