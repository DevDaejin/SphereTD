using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBase : MonoBehaviour
{
    //EnemyConfig config;
    NavMeshAgent agent;

    private void Start()
    {
        agent ??= GetComponent<NavMeshAgent>();

        if(agent == null)
        {
            Debug.LogWarning($"{gameObject.name} does not include the NavMeshAgent component. Adding it now.");
            agent = gameObject.AddComponent<NavMeshAgent>();
        }

        Init();
    }

    private void Init()
    {
        //agent.speed = config.Speed;
        agent.SetDestination(new Vector3(30, 0, 30));
    }

    private void Update()
    {
        if (agent != null) { Debug.Log(agent.pathStatus); }
    }
}
