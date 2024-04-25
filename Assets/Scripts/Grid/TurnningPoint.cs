using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class TurnningPoint : MonoBehaviour
{
    TMP_Text turnningPointNameTxt;
    [SerializeField] Vector3 nextPoint;

    const string ENEMY = "Enemy";

    public void Init(string turnningPointName, Vector3 nextPoint)
    {
        turnningPointNameTxt = GetComponent<TMP_Text>();
        turnningPointNameTxt.text = turnningPointName;

        gameObject.name = turnningPointName;
        Debug.Log($"{turnningPointName}  {nextPoint}");
        this.nextPoint = nextPoint;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("qqq");
        if (other.tag.Equals(ENEMY))
        {
            Debug.Log(nextPoint);
            other.GetComponent<NavMeshAgent>().SetDestination(nextPoint);
        }
    }
}
