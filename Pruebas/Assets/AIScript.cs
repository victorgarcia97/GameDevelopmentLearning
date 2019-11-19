using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIScript : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform player;
    private NavMeshAgent agent;
    private bool isOnSigth;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        //agent.Warp(gameObject.transform.position);
        isOnSigth = false;
        
        
    }

    // Update is called once per frame
    void Update()
    {

        checkDistance();
        if (isOnSigth)
        {
            agent.destination = player.position;
        }
        
    }

    void checkDistance()
    {
        float distance;
        distance = Vector3.Distance(gameObject.transform.position, player.transform.position);
        if (distance < 10) {
            isOnSigth = true;
        }

    }
}
