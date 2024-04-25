using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBase : MonoBehaviour
{
    NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(new Vector3( 30, 0, 30));
    }

    private void Update()
    {
        if (agent != null) { Debug.Log(agent.pathStatus); }
    }
}
