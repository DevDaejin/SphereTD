using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBase : MonoBehaviour
{
    //private EnemyConfig config;
    
    private NavMeshAgent agent;
    private int hp;

    public Action OnDeathCallback;
    public Action OnArriveCallback;

    private void Start()
    {
        agent ??= GetComponent<NavMeshAgent>();

        if(agent == null)
        {
            Debug.LogWarning($"{gameObject.name} does not include the NavMeshAgent component. Adding it now.");
            agent = gameObject.AddComponent<NavMeshAgent>();
        }

        Init(1);
    }

    private void Init(int wave)
    {
        //agent.speed = config.Speed;
        //hp = config.GetHP(wave);
        //agent.SetDestination(new Vector3(30, 0, 30));
    }

    public void TakeDamage(int damage)
    {
        // 디펜스 관련 처리 필요
        
        hp -= damage;

        if(hp <= 0) Death();
    }

    public void Death()
    {
        agent.isStopped = true;
        OnDeathCallback?.Invoke();
    }

    public void Arrive()
    {
        OnArriveCallback?.Invoke();
        Death();
    }

    private void Update()
    {
        //if (agent != null) { Debug.Log(agent.pathStatus); }
    }
}
